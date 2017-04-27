  \ parsing.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704271706
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to parsing.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( defined? parse-char parse-all parse-name-thru )

[unneeded] defined?

?\ : defined? ( ca len -- f ) undefined? 0= ;

[unneeded] parse-char

?\ : parse-char ( "c"  -- c ) stream drop c@ 1 parsed ;
  \ Parse the next char in the input stream and return its
  \ code.

[unneeded] parse-all ?(

: parse-all ( "ccc" -- ca len )
  stream dup parsed >stringer ; ?)

  \ doc{
  \
  \ parse-all ( "ccc" -- ca len )
  \
  \ Parse the rest of the source.
  \
  \ ``parse-all`` is a useful factor of Specforth editor's
  \ `text`.
  \
  \ }doc

: parse-name-thru ( "name" -- ca len )
  begin   parse-name dup 0=
  while   2drop refill 0= #-289 ?throw
  repeat ;

  \ doc{
  \
  \ parse-name-thru  ( "name" -- ca len )
  \
  \ Parse _name_ and return it as string _ca len_ within the
  \ input buffer. If the parse area is empty, use `refill` to
  \ fill it from the input source. If the input source is
  \ exhausted, throw exception #-289 (input source exhausted).
  \
  \ See also: `parse-name`, `parse`.
  \
  \ }doc

( execute-parsing string>source evaluate )

[unneeded] string>source ?(
: string>source ( ca len -- )
  blk off  (source-id) on  set-source ; ?)

  \ doc{
  \
  \ string>source ( ca len -- )
  \
  \ Set the string _ca len_ as the current source.
  \
  \ See also: `set-source`, `(source-id)`.
  \
  \ }doc

[unneeded] execute-parsing ?( need need-here
need-here string>source
: execute-parsing ( ca len xt -- )
  nest-source >r string>source r> execute unnest-source ; ?)

  \ doc{
  \
  \ execute-parsing ( ca len xt -- )
  \
  \ Make _ca len_ the current input source, execute _xt_, then
  \ restore the previous input source.
  \
  \ See also: `evaluate`, `interpret`, `string>source`,
  \ `nest-source`.
  \
  \ Origin: Gforth.
  \
  \ }doc

[unneeded] evaluate ?( need need-here
need-here execute-parsing
: evaluate ( i*x ca len -- j*x )
  ['] interpret execute-parsing ; ?)

  \ doc{
  \
  \ evaluate ( i*x ca len -- j*x )
  \
  \ Save the current input source specification. Store
  \ minus-one (-1) in `source-id`. Make the  string described
  \ by _ca len_ both the input source and input buffer,  set
  \ `>in` to zero,  and interpret.  When the  parse area  is
  \ empty, restore the prior input source specification.
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `interpret`, `execute-parsing`.
  \
  \ }doc

( char [char] word )

[unneeded] char
?\ : char ( "name" -- c ) parse-name drop c@ ;

  \ doc{
  \
  \ char ( "name" -- c )
  \
  \ Parse _name_ and put the value of its first character on
  \ the stack.
  \
  \ Solo Forth recognizes the standard notation for
  \ characters, so ``char`` is not needed:
  \
  \ ----
  \ 'x' emit .(  equals ) char x emit
  \ ----
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `[char]`.
  \
  \ }doc

[unneeded] [char]  ?(

: [char] ( "name" -- c )
  char postpone cliteral ; immediate compile-only ?)

  \ doc{
  \
  \ [char]
  \
  \ Compilation: ( "name" -- )
  \
  \ Parse "name" and append the run-time semantics given below
  \ to the current definition.
  \
  \ Run-time: ( -- c )
  \
  \ Place _c_, the value of the first character of _name_, on
  \ the stack.
  \
  \ ``[char]`` is an `immediate` and `compile-only` word.
  \
  \ Solo Forth recognizes the standard notation for
  \ characters, so ``[char]`` is not needed:
  \
  \ ----
  \ : test ( -- ) 'x' emit ."  equals " [char] x emit ;
  \ ----
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See also: `char`.
  \
  \ }doc

  \ Credit:
  \
  \ Code from Z88 CamelForth.

[unneeded] word ?(

: word ( c "<chars>ccc<char>" -- ca )
  dup  stream                 ( c c ca len )
  dup >r   rot skip           ( c ca' len' )
  over >r  rot scan           ( ca" len" )
  dup if  char-  then         \ skip trailing delimiter
  r> r> rot -   >in +!        \ update `>in`
  tuck - ( ca' len ) here place  here ( ca )
  bl over count + c! ; ?)     \ append trailing blank

  \ doc{
  \
  \ word ( c "<chars>ccc<char>" -- ca )
  \
  \ Skip leading _c_ character delimiters from the input
  \ stream.  Parse the next text characters from the input
  \ stream, until a delimiter _c_ is found, storing the packed
  \ character string beginning at _ca_ (which is the current
  \ address returned by `here`), as a counted string (the
  \ character count in the first byte), and with one blank at
  \ the end (not included in the count).
  \
  \ This word is obsolescent. Its function is superseeded by
  \ `parse` and `parse-name`.
  \
  \ NOTE: The requirement to follow the string with a space is
  \ obsolescent and was included in Forth-94 as a concession to
  \ existing programs that use ``convert`` (superseded by
  \ `>number`).  A program shall not depend on the existence of
  \ the space. The requirement to follow the string with a
  \ space was removed from Forth-2012.
  \
  \ Origin: Forth-79 (Required Word Set), Forth-83 (Required
  \ Word Set), Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ }doc

( save-input restore-input )

  \ XXX UNDER DEVELOPMENT
  \
  \ 2016-01-01: Code copied from m3Forth:
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/core-ext.f

: save-input ( -- xn ... x1 n )
  source-id 0>
  if tib #tib @ 2dup c/l 2 + allocate throw dup >r swap cmove
     r> to tib  >in @
     source-id file-position throw  5
  else blk @ >in @ 2 then ;

: restore-input ( xn ... x1 n -- f ) source-id 0>
  if dup 5 <> if 0 ?do drop loop -1 exit then
     drop source-id reposition-file ?dup
     if >r 2drop drop r> exit then
     >in ! #tib ! to tib false
  else dup 2 <> if 0 ?do drop loop -1 exit then
     drop >in ! blk ! false
  then ;

  \ ===========================================================
  \ Change log

  \ 2015-09-13: Add `parse-char`.
  \
  \ 2015-10-06: Move `word` from the kernel.
  \
  \ 2015-10-18: Move `command` from the editor and rename it to
  \ `parse-line`.
  \
  \ 2015-10-22: Fix `parse-char`.
  \
  \ 2016-04-24: Move `char` and `[char]` from the kernel.
  \
  \ 2016-05-10: Fix `[char]`.
  \
  \ 2016-05-14: Compact the blocks. Fix `parse-line` and rename
  \ it to `parse-all`. Finish and document `execute-parsing`.
  \ Move `string>source` and `evaluate` from the kernel.
  \ Rewrite `evaluate` after `execute-parsing`.
  \
  \ 2016-05-31: Update: `cliteral` has been moved to the
  \ kernel.
  \
  \ 2016-11-17: Fix needing `char`, `[char]` and `word`.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-12: Update the names of `stringer` words.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-19: Add `parse-name-thru`. Improve documentation.
  \
  \ 2017-04-27: Fix needing of `[char]`.

  \ vim: filetype=soloforth
