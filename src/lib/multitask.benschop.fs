  \ multitask.benschop.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT -- not finished

  \ Last modified: 201806041110
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Multitask support, adapted from Spectrum Forth-83.

  \ ===========================================================
  \ Authors

  \ L.C. Benschop wrote the original code Spectrum Forth-83,
  \ 1988.
  \
  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( benschop-multitasker )

need >name  need user  user (wait  ' noop (wait !

code switch
  rptr ldhl h push d push h clr sp addp exde uptr
  ldhl h dec d m ld h dec e m ld h dec begin h dec m d ld h
  dec m e ld exde m a ld a or z until
  h inc m e ld h inc  m d ld h inc uptr sthl  exde ldsp
  d pop h pop rptr  sthl jpix end-code

27028 constant uptr
variable task-link
variable first-task

-->

( benschop-multitasker )

: task: ( "name" -- )
  create here $243 + , ( eerste adres ip)
  task-link @ ,  here task-link ! ( link naar vorige taak)
  here first-task @ ! ( maak cirkel rond)
  1 c, 0 , ( nog niet starten&ruimte sp)
  uptr @ here $3C cmove  here 23c + here $0E + ! here
  $013C + here $10 + ! $023C allot ( user-variabelen+stack)
  smudge ] current @ context ! !csp -->

( benschop-multitasker )

  does> dup $15 + @ 4 - ( stackpointer)
  2dup swap 5 + !  2dup swap @ swap ! ( ip op stack)
  over $17 + @ swap 2+ ! ( rp op stack)
  0 swap 4 + c! ( runnable) ;

: terminate 2 uptr @ 3 - c!  switch ;
: sleep     3 uptr @ 3 - c!  switch ;
: stop ( tid -- ) 4 swap 7 + c! ;
: start ( tid -- ) 0 swap 7 + c! ;

: ;task compile terminate ?csp smudge [compile] [ ; immediate

task: main-task
 cr ." multi-tasking operating system"
 begin cr &> emit query
 interpret state @ 0= if ." ok" then 0 until ;task

' main-task 5 + first-task !  ' main-task 7 + task-link !
task-link @ first-task @ !  -->

( benschop-multitasker )

code (start
  ' switch h ldp# (wait sthl first-task @ 5 +
  h ldp# uptr sthl h dec m d ld h dec m e ld exde ldsp d pop
  h pop rptr sthl jpix end-code

: startup ( -- ) main-task (start ;

: tasks ( -- )
  uptr @ dup 27039 = cr if  ." multitasking not active"  else
  dup 10 - >name id. 6 emit ." active"
  begin  5 - @ 3 + dup uptr @ -  while
    cr dup 10 - >name id. 6 emit dup 3 -
    c@ dup 0 = if ." runnable" then
       dup 1 = if ." new"      then
       dup 2 = if ." terminated" then
       dup 3 = if ." sleeping" then
           4 = if ." stopped" then  repeat then drop ;

  \ ===========================================================
  \ Change log

  \ 2016-05-17: Start.
  \
  \ 2016-12-20: Move the `(wait)` user variable from the
  \ kernel.
  \
  \ 2017-12-12: Need `>name`, which has been moved to the
  \ library.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth
