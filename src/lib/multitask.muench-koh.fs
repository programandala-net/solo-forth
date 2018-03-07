  \ multitask.muench-koh.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT -- not finished

  \ Last modified: 201803072256
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Multitask support, adapted from eForth and hForth.
  \
  \ Reference: Forth Dimensions (volume 18, number 2, page 32).

  \ ===========================================================
  \ Authors

  \ Bill Muench wrote the original code for eForth, 1993-1997.
  \
  \ Wonyong Koh adapted it to hForth, 1995, 1997.
  \
  \ Marcos Cruz (programandala.net) wrote a version for Solo
  \ Forth, based on eForth and hForth, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( muench-koh-multitasker )

get-current forth-wordlist set-current need user

  \ Structure of a task created by TASK:
  \
  \ userP (points `follower`)
  \ return_stack
  \ data_stack
  \ user_area
  \ user1
  \ taskName
  \ throwFrame
  \ tos
  \ status
  \ follower
  \ sp0
  \ rp0

  \ user u1        \ free
  \ user tf        \ throw frame
  \ user tid       \ back link tid
user tos       \ top of stack
user status    \ `branch` or `wake`
user follower  \ address of next task's `status`

: pause ( -- )
  rp@ sp@ tos !  follower @ >r ; compile-only

  \
  \ pause ( -- )
  \
  \ Allow another task to execute:
  \ Stop current task and transfer control to the task of which
  \ `status` user variable is stored in `follower` user variable
  \ of current task.
  \

: wake ( -- )
  r> userp !  \ `userp` points `follower` of current task
  tos @ sp! rp! ; compile-only

  \
  \ wake ( -- )
  \
  \ Wake current task.
  \

: stop ( -- ) ['] branch status ! pause ;

  \
  \ stop ( -- )
  \
  \ Sleep current task.
  \

: 's ( tid a -- a' )
  \ follower  cell+ - swap @ + ;  \ XXX TMP -- eForth
  userP @ - swap ( offset tid ) @ + ;

  \
  \ 's ( tid a -- a' )
  \
  \ Index another task's user variable.
  \

: sleep ( tid -- ) status 's  ['] branch  swap ! ;

  \
  \ Sleep another task.
  \


: awake ( tid -- ) status 's  ['] wake  swap ! ;

  \
  \
  \ awake ( tid -- )
  \
  \ Wake another task.
  \

-->

( muench-koh-multitasker )

  \ XXX TMP -- eForth version of `task` (called `hat`):
 \ : HAT ( u s r "name" -- ) ( -- tid )
 \   CREATE + SWAP
 \   [ D# 7 CELLS ] LITERAL + ( TF\TID\TOS\STATUS\FOLLOWER\r>--<s )
 \   DUP HERE + ( rp0 ) , +
 \   DUP HERE + ( sp0 ) , ALLOT ;


: task ( user_size ds_size rs_size "name" -- )
  create here >r  \ user_size ds_size rs_size  r: tid
  0 ,  \ reserve space for `userp` pointer
  allot here cell- >r ( user_size ds_size ) ( r: tid rp0 )
  allot here cell- >r ( user_size ) ( r: tid rp0 sp0 )
  [ 6 cells ] literal + allot  \ minimum user variables
  here cell- ( user_pointer   ) ( r: tid rp0 sp0 )
  r> , r> , ( store sp0 and rp0  )
  r@ !  \ store `userp` pointer
  lastname r> taskname 's ! ;
  \ store task name in new task's 'taskname'

  \
  \ task ( user_size ds_size rs_size "name" -- )
  \
  \ Create a new task.
  \

-->

( muench-koh-multitasker )


: build ( tid -- )
  dup sleep                       \ sleep new task
  follower @ over follower 's !   \ link new task
  status 's follower ! ;         \ link old task

  \
  \ Initialize and link new task into `pause` chain.
  \

: activate ( tid -- )

  \ XXX TMP -- eForth:
  dup 2@        ( tid sp rp )

  \ XXX TMP -- hForth:
  \ dup @ cell+ 2@ cell-  \ top of stack is in bx register
  \ swap      ( tid sp0 rp0 )

  r> over !      \ save entry at rp
  over !         \ save rp at sp
  over tos 's !  \ save sp in tos
  awake ; compile-only

  \
  \ activate ( tid -- )
  \
  \ Activate the task identified by _tid_. `activate` must be
  \ used only in definition. The code following `activate` must
  \ not `exit`. In other words it must be an infinite loop like
  \ `quit`.
  \

-->

( muench-koh-multitasker )


: tasks ( -- )
  follower      \ current task's follower
  begin
    cr dup [ taskname follower - ] literal + @ .name
    dup cell- @ ['] wake =
    if  ." awaked "  else  ." sleeping "  then
    @ cell+     \ next task's follower
      dup follower =
  until drop cr ;

  \
  \ tasks ( -- )
  \
  \ Display tasks list in status-follower chain.
  \

set-current

  \ ===========================================================
  \ Change log

  \ 2016-05-17: Start.
  \
  \ 2017-05-07: Prevent the documentation from being included
  \ in the glossary, until the code is ready.
  \
  \ 2018-03-07: Remove doc markup.

  \ vim: filetype=soloforth
