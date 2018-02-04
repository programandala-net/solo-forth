  \ exception.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802041945
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The management of exceptions.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ?compiling ?executing abort" warning" )

[unneeded] ?compiling
?\ : ?compiling ( -- ) compiling? 0= #-14 ?throw ;

  \ doc{
  \
  \ ?compiling ( -- ) "question-compiling"
  \
  \ If not compiling, `throw` exception #-14 ("interpreting a
  \ compile-only word").
  \
  \ See: `compile-only`, `?executing`.
  \
  \ }doc

[unneeded] ?executing
?\ : ?executing ( -- ) compiling? #-263 ?throw ;

  \ doc{
  \
  \ ?executing ( -- ) "question-executing"
  \
  \ If not executing, `throw` exception #-263 ("execution
  \ only").
  \
  \ See: `?compiling`.
  \
  \ }doc

  \ Credit:
  \
  \ The code of `warning"` was adapted from pForth and modified.

[unneeded] abort" ?(

  \ Credit:
  \
  \ The code of `abort"` was adapted from DZX-Forth.

: (abort") ( x -- ) r> count rot if   abort-message 2! -2 throw
                                 then + >r ;

  \ doc{
  \
  \ (abort")  ( x -- ) "paren-abort-quote"
  \
  \ If _x_ is not zero, perform the function of ``-2 throw``,
  \ displaying the string that was compiled inline by `abort"`.
  \
  \ ``(abort")`` is the run-time procedure compiled by
  \ `abort"`.
  \
  \ See: `throw`.
  \
  \ }doc

: abort"
  \ Compilation: ( "ccc<quote>" -- )
  \ Run-time:    ( x -- )
  postpone (abort") ," ; immediate compile-only ?)

  \ doc{
  \
  \ abort" "abort-quote"
  \   Compilation: ( "ccc<quote>" -- )
  \   Run-time:    ( x -- )
  \
  \ Compile `(abort")`, parse _ccc_ delimited by a double
  \ quote and compile it.
  \
  \ ``abort"`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-79 (Reference Word Set), Forth-83 (Required
  \ Word Set), Forth-94 (EXCEPTION EXT), Forth-2012 (EXCEPTION
  \ EXT).
  \
  \ See: `abort-message`, `abort`, `throw`, `warning"`.
  \
  \ }doc

[unneeded] warning?( ?( need string-parameter

: (warning") ( f -- ) string-parameter rot if   type
                                           else 2drop then ;

  \ doc{
  \
  \ (warning") ( f -- ) "paren-warning-quote"
  \
  \ If _f_ is not zero, display the in-line string; else do
  \ nothing.
  \
  \ ``(warning")`` is the inner procedure compiled by
  \ `warning"`.
  \
  \ }doc

: warning"
  \ Compilation: ( "ccc<quote>" -- )
  \ Execution:   ( f -- )
  postpone (warning") ," ; immediate compile-only ?)

  \ doc{
  \
  \ warning" "warning-quote"
  \   Compilation: ( "ccc<quote>" -- )
  \   Execution:   ( f -- )

  \
  \ Compilation:
  \
  \ Parse and compile _ccc_ delimited by a double quote.
  \
  \ Execution:
  \
  \ If _f_ is not zero, display the compiled message _ccc_;
  \ else do nothing.
  \
  \ }doc

( error>ordinal error>line errors-block .throw-message )

: error>ordinal ( -n1 -- +n2 )
  abs dup 256 < ?exit  \ standard
      dup 1000 < if  [ 256 91 - ] literal - exit  then  \
      [ 1000 300 - 256 91 - + ] literal - ;
  \ Legend:
  \   -90 = lowest error defined
  \         in the standard range (-255..-1)
  \  -300 = lowest error reserved for the Forth system
  \         (not including the DOS)
  \         in the standard range (-4095..-256)
  \ -1000 = first (highest) error reserved for the DOS
  \         in the standard range (-4095..-256)

  \ doc{
  \
  \ error>ordinal ( -n1 -- +n2 ) "error-to-ordinal"
  \
  \ Convert an error code _n1_ to its ordinal position _+n2_ in
  \ the library.

  \ ----
  \ -n1 =    -90 ..   -1 \ Standard error codes
  \         -300 .. -256 \ Solo Forth error codes
  \        -1024 ..-1000 \ G+DOS error codes
  \ +n2 =      1 ..  146
  \ ----

  \ See: `error>line`.
  \
  \ }doc

: error>line ( -n1 -- n2 )
  error>ordinal dup >r
  begin  dup dup l/scr / - r@ <>  while  1+  repeat  rdrop ;

  \ doc{
  \
  \ error>line ( -n1 -- n2 ) "error-to-line"
  \
  \ Convert error code _-n1_ to line _n2_ relative to the block
  \ that contains the error messages.
  \
  \ See: `error>ordinal`.
  \
  \ }doc

need .line

variable errors-block
s" Standard error codes" located errors-block !

  \ doc{
  \
  \ errors-block ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ block that holds the error messages.
  \
  \ The variable is initialized during compilation with the
  \ first block that contains "Standard error codes" in its
  \ first line.
  \
  \ See: `.throw-message`.
  \
  \ }doc

: .throw-message ( n -- )
  errors-block @ if   cr error>line errors-block @ .line space
                 else .throw# then ;

' .throw-message ' .throw defer!

  \ doc{
  \
  \ .throw-message ( n -- ) "dot-throw-message"
  \
  \ Extended action of the deferred word `.throw`: Display the
  \ text of the `throw` exception code _n_.  The variable
  \ `errors-block` contains the number of the first block where
  \ messages are hold. If `errors-block` contains zero, only
  \ the error number is displayed.
  \
  \ See: `error>line`.
  \
  \ }doc

( catch )

: catch ( xt -- exception# | 0 )
  sp@ >r        ( xt ) \ save data stack pointer
  catcher @ >r  ( xt ) \ save previous catcher
  rp@ catcher ! ( xt ) \ set current catcher
  execute       ( )    \ `execute` returns if no `throw`
  r> catcher !  ( )    \ restore previous catcher
  r> drop       ( )    \ discard saved stack pointer
  0 ;           ( 0 )  \ normal completion, no error

  \ Credit:
  \
  \ Code from DZX-Forth, MPE Forth for TiniARM, and the
  \ Forth-2012 documentation.

  \ doc{
  \
  \ catch ( i*x xt -- j*x 0 | i*x n )
  \
  \ Push an exception frame on the exception stack and then
  \ execute _xt_ (as with `execute`) in such a way that control
  \ can be transferred to a point just after `catch` if `throw`
  \ is executed during the execution of _xt_.
  \
  \ If the execution of _xt_ completes normally (i.e., the
  \ exception frame pushed by this `catch` is not popped by an
  \ execution of `throw`) pop the exception frame and return
  \ zero on top of the data stack, above whatever stack items
  \ would have been returned by the execution of _xt_.
  \ Otherwise, the remainder of the execution semantics are
  \ given by `throw`.
  \
  \ Origin: Forth-94 (EXCEPTION), Forth-2012 (EXCEPTION).
  \
  \ See: `catcher`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09: Main development.
  \
  \ 2015-10: Fixes.
  \
  \ 2016-04-14: Restored the file from the repository. It was
  \ removed from version 0.3.0+2016-04-09 by mistake. Updated
  \ the headers and documentation.  Renamed `(.throw)` to
  \ `.throw-message`, and `msg-scr` to `error-messages-block`.
  \ Fixed `error>ordinal`.
  \
  \ 2016-04-25: Add carriage return before the exception
  \ message in `.throw-message`.
  \
  \ 2016-04-29: Add `warning"`.
  \
  \ 2016-05-03: Document `warning"`.
  \
  \ 2016-08-05: Rename `error-messages-block` to
  \ `errors-block`. Combine blocks to save two of them.
  \
  \ 2016-11-26: Move `catch` from the kernel.
  \
  \ 2017-01-13: Improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`.
  \
  \ 2017-01-20: Fix `error>ordinal`: error code #-286 was added
  \ some days ago, making #-300 the new highest code available
  \ for system error codes, but `error>ordinal` wasn't updated.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-12-08: Update source style (spacing). Improve
  \ documentation.
  \
  \ 2017-12-09: Remove optional definition of `abort-message`,
  \ which is defined in the kernel.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth

