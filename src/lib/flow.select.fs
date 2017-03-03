  \ flow.select.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ `select`.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-23: Move `select-test` to the tests module.

( select )

  \ XXX UNDER DEVELOPMENT

  \ XXX FIXME `when` causes
  \ #-22 control structure mismatch

  \ Credit:
  \
  \ Code adapted from Galope.
  \ Original code from:
  \ http://dxforth.netbay.com.au/miser.html

  \ Syntax

  \ select ( x0 )
  \    cond  <tests>  when    ... else
  \          <test>   if drop ... else
  \    ...   ( default )
  \ endselect

  \ All clauses are optional.

  \ <tests> may consist of one or more of the following:

  \  x1    equal ( test if x0 and x1 are equal )
  \  x1 x2 range ( test if x0 is in the range x1..x2 )

  \ <test> can be any code that leaves x0 and a flag (0|<>0).
  \ 'if drop ... else' is for expansion, allowing user-defined
  \ tests.

  \ 'continue' may be placed anywhere within:

  \ when ... else
  \ if ( drop ) ... else

  \ 'continue' redirects program flow from previously matched
  \ clauses that would otherwise pass to 'endselect'. It
  \ provides "fall-through" capability akin to C's switch
  \ statement.

0 constant select immediate
0 constant cond immediate

  \ XXX NOTE: A version of `thens` is in the kernel of
  \ DZX-Forth.

: thens  begin  ?dup while  postpone then  repeat ;
  \ ( 0 a'1 ... a'n -- )
  \ Compile the pending `then`.
  \ XXX TODO -- probably, common factor with `endcase`

: endselect  postpone drop  thens ; immediate
  \ ( Compilation: 0 a'1 ... a'n -- ) ( Run-time: x0 -- )

: when
  \ ( Compilation: 0 orig1..orign -- )
  \ ( Run-time: xxx )
  postpone else  >r >r >r  thens  r> r> r>  postpone drop
 ; immediate
  \ XXX TODO stack

: continue
  \ ( Compilation: xxx )
  \ ( Run-time: xxx )
  >r >r >r thens  0  r> r> r> ; immediate
  \ XXX TODO stack

: equal
  \ ( Compilation: -- orig )
  \ ( Run-time: x0 x1 -- )
  postpone over  postpone -  postpone if ; immediate

: (range) ( x0 x1 x2 -- x0 f )
  2>r dup 2r> over - -rot - u< ;

: range
  \ ( Compilation: -- orig )
  \ ( Run-time: x0 x1 x2 -- x0 f )
  postpone (range)  postpone if ; immediate  -->

  \ vim: filetype=soloforth