  \ tool.list.stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to examine the stack.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( depth .depth .s u.s )

unneeding depth

?\ : depth  ( -- +n ) sp@ sp0 @ - [ cell negate ] literal / ;

  \ doc{
  \
  \ depth  ( -- +n )
  \
  \ _+n_ is the number of single-cell values contained in the
  \ data stack before _+n_ was placed on the stack.
  \
  \ Origin: Forth-79 (Required Word Set), Forth-83 (Required Word
  \ Set), Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `sp@`, sp0`, `cell`, `rdepth`, `fdepth`, `.depth`.
  \
  \ }doc

unneeding .depth ?\ : .depth ( n -- ) ." <" 0 .r ." > " ;

  \ doc{
  \
  \ .depth ( n -- )
  \
  \ Display _n_ with the format used by `.s` and `u.s` to
  \ display the `depth` of the data stack`.
  \
  \ See also: `.r`, `depth`.
  \
  \ }doc

unneeding .s ?( need depth need .depth need +loop need do

defer (.s ( x -- ) ' . ' (.s defer!

: .s ( -- )
  depth dup .depth 0> 0exit
  sp@ sp0 @ cell- do i @ (.s [ cell negate ] literal +loop ; ?)

  \ Credit:
  \ Code from Afera. Original algorithm from v.Forth.

  \ doc{
  \
  \ .s ( -- )
  \
  \ Display, using `.`, the values currently on the data stack.
  \
  \ See also: `u.s`, `depth`, `.depth`.
  \
  \ }doc

unneeding u.s ?( need .s

: u.s   ( -- )
  ['] u. ['] (.s defer!  .s  ['] . ['] (.s defer! ; ?)

  \ doc{
  \
  \ u.s ( -- )
  \
  \ Display, using `u.`,  the values currently on the data
  \ stack.
  \
  \ See also: `.s`, `depth`, `.depth`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-13: Modified `.depth` to print a signed number,
  \ better for debugging.
  \
  \ 2016-04-12: Divided into 3 blocks, in order to reuse
  \ `.depth` for the floating point `.fs`. Fixed the check: the
  \ stacks are not printed when their depth is negative.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-12-30: Compact the code, saving two blocks. Make
  \ `.depth` 4 bytes smaller.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-25: Add deferred `(.s)` to `.s` and rewrite `u.s`
  \ after `.s`.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.
  \
  \ 2020-05-04: Improve documentation.
  \
  \ 2020-05-09: Move `depth` from the kernel. Update the
  \ corresponding requirements.
  \
  \ 2020-05-18: Update: `+loop` was moved to the library.
  \
  \ 2020-06-06: Fix `.s`.
  \
  \ 2020-06-08: Update: now `0exit` is in the kernel.

  \ vim: filetype=soloforth
