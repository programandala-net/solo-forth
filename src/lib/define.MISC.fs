  \ define.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202005190016
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Miscellaneous definers and related words that can be
  \ defined in less than one block.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( create: :noname nextname )

unneeding create:

?\ : create: ( "name" -- ) create hide ] ;

  \ Credit:

  \ The idea for `create:` was borrowed from CP/M-volksForth
  \ 3.80a.

  \ doc{
  \
  \ create: ( "name" -- ) "create-colon"
  \
  \ Create a word _name_ which is compiled as a colon word but,
  \ when executed, will return the address of its data field
  \ address.
  \
  \ }doc

unneeding :noname ?(

: :noname ( -- xt )
  here  dup lastxt !  last off  !csp
  docolon [ assembler-wordlist >order ] call, [ previous ]
  noname? on  ] ; ?)

  \ doc{
  \
  \ :noname ( -- xt ) "colon-no-name"
  \
  \ Create an execution token _xt_. Enter compilation state and
  \ start the current definition, which can be executed later
  \ by using _xt_.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See: `nextname`.
  \
  \ }doc

unneeding nextname ?( need 2variable 2variable nextname-string

  \ doc{
  \
  \ nextname-string ( -- a )
  \
  \ A `2variable`. _a_ is the address of a double-cell
  \ containing the address and length of a name to be used by
  \ the next defining word.  This variable is set by
  \ `nextname`.
  \
  \ Origin: Gforth.
  \
  \ See: `nextname-header`.
  \
  \ }doc

: nextname-header ( -- )
  nextname-string 2@ header, default-header ;

  \ doc{
  \
  \ nextname-header ( -- )
  \
  \ Create a dictionary header using the name string set by
  \ `nextname`.  Then restore the default action of `header`.
  \
  \ Origin: Gforth.
  \
  \ See: `nextname-string`.  `default-header`.
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
  \ defining word will leave the input stream alone.
  \ ``nextname`` works with any defining word.
  \
  \ Origin: Gforth.
  \
  \ See: `nextname-header`, `nextname-string`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Move `:noname` from the library.  Move `;code`
  \ to the library.
  \
  \ 2016-11-16: Rename `code-field,` `call,` in `:noname`,
  \ after the changes in the kernel.

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
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-02-28: Fix typo in documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-12-10: Add missing `asm` to `;code`. Improve
  \ documentation.
  \
  \ 2017-12-14: Improve documentation.
  \
  \ 2017-12-15: Move `;code` to <assembler.MISC.fs>.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-04-11: Update notation "double variable" to
  \ "double-cell variable".
  \
  \ 2018-06-04: Link `2variable` in documentation.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library.

  \ vim: filetype=soloforth
