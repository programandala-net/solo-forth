  \ define.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ Miscellaneous definers that can be defined in less than one
  \ block.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-11-26: Create this module to combine the modules that
  \ contain small definers, in order to save blocks:
  \ <define.semicolon.code.fsb>, <define.colon-no-name.fsb>,
  \ <define.colon-nextname.fsb>, <flow.create-colon.fsb>.
  \
  \ 2016-11-26: Improve documentation of `nextname` and family.
  \
  \ 2016-12-06: Improve documentation of `:noname`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-05: Update `also assembler` to `assembler-wordlist
  \ >order`.
  \
  \ 2017-01-07: Improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \ Update cross references.

( create: ;code :noname nextname )

[unneeded] create:
?\ : create: ( "name" -- ) create hide ] ;

  \ Credit:

  \ The idea for `create:` was borrowed from CP/M-volksForth
  \ 3.80a.

  \ doc{
  \
  \ create: ( "name" -- )
  \
  \ Create a word "name" which is compiled as a colon word but,
  \ when executed, will return the address of its pfa.
  \
  \ }doc

[unneeded] ;code ?(

  \ Old history:
  \
  \ 2016-04-24: Move `;code` to the library.

: ;code ( -- ) postpone (;code)  finish-code
 ; immediate compile-only ?)

  \ XXX TODO -- Improve documentation.

  \ doc{
  \
  \ ;code ( -- )
  \
  \ Stop compilation and terminate a new defining word by
  \ compiling the run-time routine `(;code)`.
  \
  \ Origin: fig-Forth, Forth-79 (Assembler Word Set), Forth-83
  \ (Assembler Extension Word Set), Forth-94 (TOOLS EXT),
  \ Forth-2012 (TOOLS EXT).
  \
  \ }doc

[unneeded] :noname ?(

  \ Old history:
  \
  \ 2016-04-24: Moved `:noname` from the library.
  \
  \ 2016-11-16: Rename `code-field,` to `call,`, after the
  \ changes in the kernel.

: :noname ( -- xt )
  here  dup lastxt !  last off  !csp
  docolon [ assembler-wordlist >order ] call, [ previous ]
  noname? on  ] ; ?)

  \ doc{
  \
  \ :noname ( -- xt )
  \
  \ Create an execution token _xt_. Enter compilation state and
  \ start the current definition, which can be executed later
  \ by using _xt_.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

[unneeded] nextname ?( 2variable nextname-string

  \ doc{
  \
  \ nextname-string ( -- a )
  \
  \ A double variable that may hold the address and length of a
  \ name to be used by the next defining word.  This variable
  \ is set by `nextname`.
  \
  \ Origin: Gforth.
  \
  \ See also: `nextname-header`.
  \
  \ }doc

: nextname-header ( -- )
  nextname-string 2@ header, default-header ;

  \ doc{
  \
  \ nextname-header ( -- )
  \
  \ Create a dictionary header using the name string set by
  \ `nextname`.  Then restore the default action of
  \ `header`.
  \
  \ Origin: Gforth.
  \
  \ See also: `nextname-string`.
  \ `default-header`.
  \
  \ }doc

: nextname ( ca len -- ) nextname-string 2!
  ['] nextname-header ['] header defer! ; ?)

  \ Credit:
  \
  \ `nextname` is borrowed from Gforth.

  \ doc{
  \
  \ nextname ( ca len -- )
  \
  \ The next defined word will have the name _ca len_; the
  \ defining word will leave the input stream alone. `nextname`
  \ works with any defining word.
  \
  \ Origin: Gforth.
  \
  \ See also: `nextname-header`, `nextname-string`.
  \
  \ }doc

  \ vim: filetype=soloforth
