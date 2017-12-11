  \ flow.select.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712111822
  \ See change log at the end of the file

  \ XXX UNDER DEVELOPMENT

  \ ===========================================================
  \ Authors

  \ Ed
  \ http://dxforth.netbay.com.au/miser.html

  \ Modified and adapted by Marcos Cruz (programandala.net),
  \ 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( select )

  \ XXX FIXME `when` causes
  \ #-22 control structure mismatch

  \ Credit:
  \
  \ Code adapted from Galope.
  \ Original code from:
  \ http://dxforth.netbay.com.au/miser.html

need cs-mark need cond need thens

: select cs-mark ; immediate compile-only
  \ Compilation: ( C: -- cs-mark )
  \ Run-time:    ( -- )


  \
  \ select
  \   Compilation: ( C: -- cs-mark )
  \   Run-time:    ( -- )

  \ Sintax:

  \ ----
  \ select ( x0 )
  \    cond  <tests>  when    ... else
  \          <test>   if drop ... else
  \    ...   ( default )
  \ endselect
  \ ----

  \ All clauses are optional.

  \ _<tests>_ may consist of one or more of the following:

  \ ----
  \  x1    equal ( test if x0 and x1 are equal )
  \  x1 x2 range ( test if x0 is in the range x1..x2 )
  \ ----

  \ _<test>_ can be any code that leaves x0 and a flag (0|<>0).
  \ ``if drop ... else`` is for expansion, allowing user-defined
  \ tests.

  \ 'continue' may be placed anywhere within:

  \ ----
  \ when ... else
  \ if ( drop ) ... else
  \ ----

  \ 'continue' redirects program flow from previously matched
  \ clauses that would otherwise pass to 'endselect'. It
  \ provides "fall-through" capability akin to C's switch
  \ statement.
  \
  \ Usage example:

  \ ----
  \ : test ( n -- )  space
  \   dup cr .
  \   select
  \     cond
  \          $00 $1F range
  \          $7F     equal when ." Control char"      else
  \     cond
  \          $20 $2F range
  \          $3A $40 range
  \          $5B $60 range
  \          $7B $7E range when ." Punctuation"       else
  \     cond $30 $39 range when ." Digit"             else
  \     cond $41 $5A range when ." Upper case letter" else
  \     cond $61 $7A range when ." Lower case letter" else
  \     ." Not a character" \ default
  \  endselect ;
  \
  \ 'a' test ',' test '8' test '?' test 'K' test
  \ 0 test 127 test 128 test
  \ ----

  \ See: `endselect`, `cond`, `equal`, `range`, `when`.
  \
  \ }doc

: endselect
  \ Compilation: ( C: cs-mark a#1 .. a#n -- )
  \ Run-time:    ( x0 -- )
  postpone drop postpone thens ; immediate compile-only


  \
  \ endselect
  \   Compilation: ( C: cs-mark a#1 .. a#n -- )
  \   Run-time:    ( x0 -- )
  \
  \ Terminate a `select` structure.
  \
  \ }doc

: when
  \ Compilation: ( C: cs-mark orig#1 .. orig#n -- )
  \ Run-time:    ( xxx )
  postpone else >r >r >r postpone thens r> r> r>
  postpone drop ; immediate compile-only
  \ XXX TODO stack


  \
  \ when
  \   Compilation: ( C: cs-mark orig#1 .. orig#n -- )
  \   Run-time:    ( xxx ) \ XXX TODO --
  \
  \ See: `select`.
  \
  \ }doc

: continue
  \ Compilation: ( C: xxx )
  \ Run-time:    ( xxx )
  >r >r >r postpone thens 0 r> r> r> ; immediate compile-only
  \ XXX TODO stack


  \
  \ continue
  \   Compilation: ( C: xxx ) \ XXX TODO --
  \   Run-time:    ( xxx ) \ XXX TODO --
  \
  \ See: `select`.
  \
  \ }doc

: equal
  \ Compilation: ( C: -- orig )
  \ Run-time:    ( x0 x1 -- x0 )
  postpone over postpone <> postpone if
  ; immediate compile-only


  \
  \ equal
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( x0 x1 -- x0 )
  \
  \ See: `select`.
  \
  \ }doc

: (range) ( x0 x1 x2 -- x0 f ) 2>r dup 2r> over - -rot - u< ;


  \
  \ (range) ( x0 x1 x2 -- x0 f )
  \
  \ The run-time procedure compiled by `range`.
  \
  \ See: `select`.
  \
  \ }doc

: range
  \ Compilation: ( C: -- orig )
  \ Run-time:    ( x0 x1 x2 -- x0 f )
  postpone (range) postpone if ; immediate compile-only


  \
  \ range
  \   Compilation: ( C: -- orig )
  \   Run-time:    ( x0 x1 x2 -- x0 f )
  \
  \ See: `select`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-23: Move `select-test` to the tests module.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-11-27: Move `cond` and `thens` to <flow.MISC.fs>.
  \
  \ 2017-12-11: Use `cs-mark`. Update layout. Improve
  \ documentation. Fix usage of `thens`.

  \ vim: filetype=soloforth
