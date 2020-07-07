  \ assembler.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007080015

  \ ===========================================================
  \ Description

  \ Z80 assembler misc words, independent from the actual
  \ assembler.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( pushhlde << >> ;code )

unneeding pushhlde ?(

get-current assembler-wordlist dup >order set-current

pushhl 1- constant pushhlde

previous set-current ?)

  \ doc{
  \
  \ pushhlde ( -- a ) "push-h-l-d-e"
  \
  \ _a_ is the address of a secondary entry point of the Forth
  \ inner interpreter.  The code at _a_ pushes registers DE and
  \ HL onto the stack and then continues at the address
  \ returned by `next`.
  \
  \ NOTE: DE is pushed first, so HL is left on top of the
  \ stack.  This is equivalent to pushing the double number
  \ formed by both registers, being HL the high part and DE the
  \ lower part.
  \
  \ ``pushhlde`` is useful for exiting from a `code` word using
  \ an absolute conditional jump.
  \
  \ See: `pusha`, `pushhl`.
  \
  \ }doc

unneeding << unneeding >> and ?(

  \ Credit:
  \
  \ Code adapted from Pygmy Forth 1.7 for DOS.
  \
  \ Original code by Frank Sergeant, for Pygmy Forth.
  \
  \ See the license at <http://pygmy.utoh.org/license.html>.

need depth need 16hex. need 8hex.

: << ( -- ca n ) here depth ;

  \ doc{
  \
  \ << ( -- ca +n ) "less-than-less-than"
  \
  \ Mark the start of a code zone to be dumped by `>>`.  _ca_
  \ is the current data-pointer and _+n_ is the current
  \ `depth`. Both of them are used by `>>`. See `>>` for a
  \ usage example.
  \
  \ Origin: Pygmy Forth.
  \
  \ }doc

: >> ( ca +n -- )
  depth 1- <> #-258 ?throw
  cr dup 16hex. here swap ?do i c@ 8hex. loop ; ?)

  \ doc{
  \
  \ >> ( ca +n -- ) "greater-than-greater-than"
  \
  \ Display starting address _ca_ as a 16-bit hexadecimal
  \ number. Then dump the code compiled in data space from _ca_
  \ to the current data-space pointer, in hexadecimal. _+n_ is
  \ used for error checking. _ca_ and _+n_ were left by _<<_.
  \
  \ ``<<`` and `>>` allow you to dump short (or long) snippets
  \ of assembly code to the screen for your inspection. If you
  \ want to see how a piece of assembly code gets assembled,
  \ just put it between the brackets.
  \
  \ Usage example:

  \ ----
  \ create useless-code-routine ( -- a )
  \   asm  << C9 c, >> end-asm
  \
  \ need assembler
  \
  \ code useless-code-word ( n1 -- n1 )
  \   << h pop, h incp, h decp, h push, jpnext, >>
  \ end-code
  \ ----

  \ Origin: Pygmy Forth.
  \
  \ See: `dump`, `wdump`, `assembler`.
  \
  \ }doc

unneeding ;code ?(

: ;code
  \ Compilation: ( -- )
  \ Run-time:    ( -- ) ( R: nest-sys -- )
  postpone (;code
  $E1 c, \ Z80 opcode for "pop hl"
  finish-code asm ; immediate compile-only ?)

  \ doc{
  \
  \ ;code "semicolon-code"
  \   Compilation: ( -- )
  \   Run-time:    ( -- ) ( R: nest-sys -- )

  \ Define the execution-time action of a word created by a
  \ low-level defining word.  Used in the form:

  \ ----
  \ : namex ... create ... ;code ... end-code
  \
  \ namex name
  \ ----

  \ where `create` could be also any user defined word which
  \ executes `create`.

  \ ``;code`` marks the termination of the defining part of the
  \ defining word _namex_ and then begins the definition of the
  \ execution-time action for words that will later be defined
  \ by _namex_.  When _name_ is called, its parameter field
  \ address is in register HL and the `assembler` code compiled
  \ between ``;code`` and `end-code` is executed.
  \
  \ Detailed description:
  \
  \ Compilation:
  \
  \ Append the run-time semantics  below  to the  current
  \ definition. End  the  current definition, allow it to be
  \ found  in the dictionary, and enter interpretation `state`.
  \
  \ Enter `assembler` mode by executing `asm`, until `end-code`
  \ is executed.
  \
  \ Run-time:
  \
  \ Replace the execution semantics of the most recent
  \ definition, which should be defined with `create` or a
  \ user-defined word that calls `create`, with the name
  \ execution semantics given  below.  Return  control  to  the
  \ calling  definition  specified by _nest-sys_.
  \
  \ Initiation: ``( i*x -- i*x dfa ) ( R: -- nest-sys2 )``
  \
  \ Save information _nest-sys2_  about the calling definition.
  \ Place _name_'s data field address _dfa_ on the stack. The
  \ stack effects _i*x_ represent arguments to name.
  \
  \ _name_ execution:
  \
  \ Perform the machine code sequence that was generated
  \ following ``;code`` and finished by `end-code`.
  \
  \ ``;code`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : border-changer ( n -- )
  \   create c, ;code ( -- ) m a ld, FE out, jpnext, end-code
  \
  \ 0 border-changer black-border
  \ 1 border-changer blue-border
  \ 2 border-changer red-border
  \ ----

  \ Which is equivalent to:

  \ ----
  \ : border-changer ( n -- )
  \   create c, does> ( -- ) ( dfa ) c@ border ;
  \
  \ 0 border-changer black-border
  \ 1 border-changer blue-border
  \ 2 border-changer red-border
  \ ----

  \ Origin: fig-Forth, Forth-79 (Assembler Word Set), Forth-83
  \ (Assembler Extension Word Set), Forth-94 (TOOLS EXT),
  \ Forth-2012 (TOOLS EXT).
  \
  \ See: `(;code`, `does>`, `asm`, `create`.
  \
  \ }doc

  \ ===========================================================--
  \ Change log

  \ 2015, 2016: Unfinished conversion of Pygmy Forth's `<<` and
  \ `>>`.
  \
  \ 2017-05-09: Rename the file. Finish, test and document the
  \ code of `<<` and `>>`.
  \
  \ 2017-05-10: Add `pushhlde`.
  \
  \ 2017-12-15: Move `;code` from <definer.MISC.fs>.
  \
  \ 2018-01-25: Fix comments.
  \
  \ 2018-02-02: Update code style of `>>`.
  \
  \ 2018-02-05: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.
  \
  \ 2020-02-28: Fix markup in documentation.
  \
  \ 2020-02-29: Fix `>>`: it displayed one extra byte, it
  \ needed just `1-` before the loop, but it has been rewritten
  \ with `?do` instead of `for`, making it simpler.
  \
  \ 2020-05-09: Update requirements: `depth` has been moved to
  \ the library.
  \
  \ 2020-07-08: Improve documentation.

  \ vim: filetype=soloforth
