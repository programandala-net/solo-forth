  \ flow.for.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702272355

  \ -----------------------------------------------------------
  \ Description

  \ `for step`.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ Credit

  \ Adapted and modified from code written by Garry Lancaster
  \ for Z88 CamelForth, 2001.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-07-06: Adapted from Z88 CamelForth.
  \
  \ 2015-08-14: Checked. Removed the fig-Forth compiler
  \ security.  Moved from the kernel to the library.  Improved
  \ after Gforth: now the index is checked before decrementing
  \ it.  Documented.
  \
  \ 2015-03-23: Added `for-i`, because `i` cannot be used since
  \ the implementation of the Forth-83 `do loop`.
  \
  \ 2016-04-16: Revised. Improved file header. Fixed stack
  \ notations.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-02-27: Improve documentation.

( for step )

  \ Code adapted from Z88 CamelForth. Modified to do the check
  \ before decrementing the index.

code (step) ( R: n -- n' )

  \ doc{
  \
  \ (step)
  \
  \ The run-time procedure compiled by `step`.
  \
  \ Run-time:    ( R: n -- n' )
  \
  \ If the loop index is zero, discard the loop parameters and
  \ continue execution after the loop. Otherwise decrement the
  \ loop index and continue execution at the beginning of the
  \ loop.
  \
  \ }doc

  2A c, rp ,
    \ ld hl,(return_stack_pointer)
  5E c, 23 c, 56 c,
    \ ld e,(hl)
    \ inc hl
    \ ld d,(hl) ; de = loop index
  7A c, B3 c,
    \ ld a,d
    \ or e ; z=already zero?
  1B c, 72 c, 2B c, 73 c,
    \ dec de
    \ ld (hl),d
    \ dec hl
    \ ld (hl),e ; update the loop index
  C2 c, ' branch ,
    \ jp nz,branch_code ; loop again if not zero
    \ ; done, discard loop index:
  23 c, 23 c, 22 c, rp ,
    \ inc hl
    \ inc hl
    \ ld (return_stack_pointer),hl
    \ ; skip branch offset and jump to next
  03 c, 03 c, jpnext,
    \ inc bc
    \ inc bc
    \ jp next

  end-code

: for ( n -- ) postpone >r <mark ; immediate compile-only

  \ doc{
  \
  \ for Compilation: ( R: -- dest ) Run-time:    ( n -- )
  \
  \ Start of a ``for``..`step` loop, that will iterate _n+1_
  \ times, starting with _n_ and ending with 0.
  \
  \ The current value of the index can be retrieved with
  \ `for-i`.
  \
  \ ``for`` is an `immediate` and `compile-only` word.
  \
  \ }doc

: step ( -- )
  postpone (step) <resolve ; immediate compile-only

  \ doc{
  \
  \ step
  \   Compilation: ( dest -- )
  \   Run-time:    ( R: n -- n' )
  \
  \ Compilation: ( dest -- )
  \
  \ Append the run-time semantics given below to the current
  \ definition. Resolve the destination of `for`.
  \
  \ Run-time: ( R: n -- n' )
  \
  \ If the loop index is zero, discard the loop parameters and
  \ continue execution after the loop. Otherwise decrement the
  \ loop index and continue execution at the beginning of the
  \ loop.
  \
  \ ``step`` is an `immediate` and `compile-only` word.
  \
  \ Note: ``step`` is usually called ``next`` in other Forth
  \ systems.
  \
  \ Origin: Z88 CamelForth.
  \
  \ }doc

need alias

' r@ alias for-i ( -- n )

  \ doc{
  \
  \ for-i ( -- n )
  \
  \ Return the current index _n_ of a `for` loop.
  \
  \ }doc

  \ vim: filetype=soloforth
