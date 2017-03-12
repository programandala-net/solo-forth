  \ flow.dfor.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702272359
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `dfor dstep`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( dfor dstep di )

: (dstep) ( R: x d -- x d' )

  \ doc{
  \
  \ (dstep) ( R: x d -- x d' | x )
  \
  \ The run-time procedure compiled by `dstep`.
  \
  \ If the loop index _ud_ is zero, discard it and continue
  \ execution after the loop. Otherwise decrement the loop
  \ index and continue execution at the beginning of the loop.
  \
  \ }doc

  r>  \ save the return address
  2r> 2dup or  \ is the index zero?
  if    -1. d+ 2>r
    \ decrement the index
  else  2drop  cell+ cell+
    \ discard the index and skip the branch offset
  then  >r ;
    \ restore the return address

: dfor ( d -- ) postpone 2>r <mark ; immediate compile-only

  \ doc{
  \
  \ dfor
  \
  \ Compilation: ( R: -- dest ) Run-time: ( d -- )
  \
  \ Start of a ``dfor``..`dstep` loop, that will iterate _ud+1_
  \ times, starting with _du_ and ending with 0.
  \
  \ ``dfor`` is an `immediate` and `compile-only` word.
  \
  \ The current value of the index can be retrieved with
  \ `dfor-i`.
  \
  \ }doc

: dstep ( -- )
  postpone (dstep) postpone branch <resolve
  ; immediate compile-only

  \ doc{
  \
  \ dstep
  \   Compilation: ( dest -- )
  \   Run-time:    ( R: ud -- ud' )
  \
  \ ``dstep`` is an `immediate` and `compile-only` word.
  \
  \ Compilation: ( dest -- )
  \
  \ Append the run-time semantics given below to the current
  \ definition. Resolve the destination of `dfor`.
  \
  \ Run-time: ( R: ud -- ud' )
  \
  \ If the loop index is zero, discard the loop parameters and
  \ continue execution after the loop. Otherwise decrement the
  \ loop index and continue execution at the beginning of the
  \ loop.
  \
  \ }doc

need alias

' 2r@ alias dfor-i ( -- d )

  \ doc{
  \
  \ dfor-i ( -- d )
  \
  \ Return the current index _d_ of a `dfor` loop.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-01: Written, a double-cell variant of `for step`.
  \
  \ 2015-03-23: Renamed `di` to `dfor-i`, after `for-i`,
  \ because `i` cannot be used since the implementation of the
  \ Forth-83 `do loop`.
  \
  \ 2016-04-16: Revised. Improved file header.  Fixed stack
  \ notations.
  \
  \ 2016-11-26: Improve `(dstep)`.
  \
  \ 2017-02-27: Improve documentation.

  \ vim: filetype=soloforth
