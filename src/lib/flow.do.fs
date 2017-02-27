  \ flow.do.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ `do` and `-do`

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2017-02-19: Move the code of `do`, `-do` and `(-do)` from
  \ the kernel.

( do -do )

[unneeded] do ?(

: do ( -- do-sys )
  postpone (do) >mark ; immediate compile-only ?)

  \ Credit:
  \ Code adapted from Spectrum Forth-83.

  \ doc{
  \
  \ do
  \   Compilation: ( -- do-sys )
  \

  \ Compile `(do)` and leave _do-sys_ to be consumed by `loop` or
  \ `+loop`.
  \
  \ Definition:

  \ ----
  \ : do ( -- do-sys )
  \   postpone (do) >mark ;  immediate compile-only
  \ ----

  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `?do`, `-do`.
  \
  \ }doc

[unneeded] -do ?(

code (-do) ( n1|u1 n2|u2 -- ) ( R: -- loop-sys )
  D1 c, E1 c, A7 c, ED c, 52 c, D2 c, ' branch , 19 c, EB c,
  \ pop de              ; init
  \ pop hl              ; limit
  \ and a
  \ sbc hl,de           ; limit<init?
  \ jp nc,branch_       ; if not, jump
  \ add hl,de           ; reverse the substraction
  \ ex de,hl            ; HL=init, DE=limit
  13 c, C3 c, ' (do) 2+ , end-code
  \ inc de              ; increment the limit
  \ jp paren_do.de_hl

  \ Credit:
  \
  \ Code based on Spectrum Forth-83' `(do)`

  \ XXX TODO -- Finish the documentation.

  \ doc{
  \
  \ (-do) ( n1|u1 n2|u2 -- ) ( R: -- loop-sys | )
  \
  \ This word is compiled by `-do`.
  \
  \ If _n1|u1_ is not less than _n2|u2_, discard both
  \ parameters and continue execution at the location given by
  \ the consumer of the _do-sys_ left by `-do` at compilation
  \ time.  Otherwise set up loop control parameters _loop-sys_
  \ with index _n2|u2_ and limit _n1|u1_ and continue executing
  \ immediately following `-do`.  Anything  already on the
  \ return stack becomes unavailable until the loop control
  \ parameters _loop_sys_ are discarded.
  \
  \ }doc

: -do ( -- do-sys )
  postpone (-do) >mark ; immediate compile-only ?)

  \ doc{
  \
  \ -do
  \   Compilation: ( -- do-sys )
  \

  \ Compile `(-do)` and leave _do-sys_ to be consumed by `loop`
  \ or `+loop`.  This word is an alternative to `do` and `?do`,
  \ to create count-down loops with `+loop`.
  \
  \ Definition:

  \ ----
  \ : -do ( -- do-sys )
  \   postpone (-do) >mark ;  immediate compile-only
  \ ----

  \ Usage example:

  \ ----
  \ : -count-down ( limit start -- )
  \   -do i . -1 +loop ;
  \
  \ 0 0 -count-down \ prints nothing
  \ 4 0 -count-down \ prints nothing
  \ 0 4 -count-down \ prints 4 3 2 1
  \
  \ \ Compare to:
  \
  \ : ?count-down ( limit start -- )
  \   ?do i . -1 +loop ;
  \
  \ 0 0 ?count-down \ prints nothing
  \ 4 0 ?count-down \ prints 0 -1..-32768 32767..4
  \ 0 4 ?count-down \ prints 4 3 2 1 0
  \
  \ : count-down ( limit start -- )
  \   do i . -1 +loop ;
  \
  \ 0 0 count-down \ prints 0
  \ 4 0 count-down \ prints 0 -1..-32768 32767..4
  \ 0 4 count-down \ prints 4 3 2 1 0
  \ ----

  \ Origin: Gforth.
  \
  \ }doc

  \ vim: filetype=soloforth

