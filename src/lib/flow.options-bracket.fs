  \ flow.options-bracket.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802071853
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `options[` control structure, an alternative to `case` with
  \ single-word options and a specific default case. The
  \ compilation of options is done in interpretation mode.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ Credit:

  \ `options[` is a port of IsForth's `case:`.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( options[ )

  \ XXX TODO -- alternative version `coptions[`

  \ Data space used: 166 bytes.

variable (default-option)
  \ Execution token of the default option.

variable #options
  \ Number of compiled options.

: default-option ( "name" -- ) ' (default-option) ! ;

  \ doc{
  \
  \ default-option ( "name" -- )
  \
  \ Set the default option "name" of an `options[ ]options`
  \ structure.  It can be anywhere inside the structure.
  \
  \ See `options[` for a usage example.
  \
  \ }doc

: (options) ( i*x x -- j*x )

  \ x = option to search for

  false swap ( false x ) \ default flag returned by the loop
  r> dup @ >r   \ set the new exit point
  cell+ dup >r  \ save the address of the default option xt
  dup cell+ @ ( false x a n )
  \ a = address of the first compiled option minus two cells
  \ n = number of compiled options

  0 do
    cell+ cell+ 2dup @ = ( false x a' f ) \ match?
    \ a' = address of the current compiled option
    if  nip nip cell+ perform  true 0 0  leave then
  loop ( f x1 x2 ) 2drop

  if    rdrop       \ match, so discard the default option
  else  r> perform  \ no match, so execute the default option
  then ;   -->

  \ doc{
  \
  \ (options) ( i*x x -- j*x ) "paren-options"
  \
  \ Run-time procedure compiled by `options[`.
  \
  \ x = option to search for
  \
  \ }doc

( options[ )

: options[

  \ Compilation: ( -- a1 a2 a3 )

  \ a1 = address of exit point
  \ a2 = address of the xt of the default option
  \ a3 = address of number of options

  (default-option) off        \ assume no default option
  #options off                \ init number of options
  postpone (options)          \ compile run-time handler
  >mark >mark >mark           ( a1 a2 a3 )
  postpone [                  \ start interpreting options
  ; immediate compile-only

  \ doc{
  \
  \ options[ "options-left-bracket"
  \
  \ Compilation: ( -- a1 a2 a3 )
  \
  \ Start an `options[ ]options` structure.
  \
  \ The addresses left on the stack will be resolved by
  \ `]options`:
  \
  \ - a1 = address of exit point
  \ - a2 = address of the xt of the default option
  \ - a3 = address of number of options
  \
  \ Usage example:
  \
  \ ----
  \ : say10      ." dek" ;
  \ : say100     ." cent" ;
  \ : say1000    ." mil" ;
  \ : say-other  ." alia" ;
  \
  \ : say ( n )
  \   options[
  \     10 option  say10
  \    100 option  say100
  \   1000 option  say1000
  \        default-option say-other
  \   ]options ;
  \
  \ 10 say  100 say  1000 say  1001 say
  \ ----
  \
  \ ``options[`` is an `immediate` and `compile-only` word.
  \
  \ }doc

: option ( x "name" -- )
  ,  ' compile,  1 #options +! ;

  \ doc{
  \
  \ option ( x "name" -- )
  \
  \ Compile the action "name" of an option _x_ in an `options[
  \ ]options` control structure.
  \
  \ See `options[` for a usage example.
  \
  \ }doc

: ]options ( a1 a2 a3 -- )
  \ a1 = address of exit point
  \ a2 = address of default option xt
  \ a3 = address of number of options
  #options @ swap !           \ store number of options
  (default-option) @ swap !   \ store default option xt
  >resolve                    \ store exit point
  ] ;

  \ doc{
  \
  \ ]options ( a1 a2 a3 -- ) "right-bracket-options"
  \
  \ End a `options[ ]options` structure. Resolve the addresses
  \ left by `options[`:
  \
  \ - a1 = address of exit point
  \ - a2 = address of default option xt
  \ - a3 = address of number of options
  \
  \ See `options[` for a usage example.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-10-15: Finish porting IsForth's `case:`, with
  \ different names.  In the original code the word `docase`
  \ (called `(options)` in this port) is written in x86
  \ assembler.  It has been rewritten from scratch, without
  \ investigating the assembler code.
  \
  \ 2016-04-29: Improve documentation.
  \
  \ 2016-11-26: Improve `(options)`.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2018-02-07: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth
