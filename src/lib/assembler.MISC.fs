  \ assembler.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705100026

  \ ===========================================================
  \ Description

  \ Z80 assembler misc words, independent from the actual
  \ assembler.

  \ ===========================================================
  \ Authors

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( pushhlde << >> )

[unneeded] pushhlde ?(

get-current assembler-wordlist dup >order set-current

pushhl 1- constant pushhlde

previous set-current ?)

  \ doc{
  \
  \ pushhlde ( -- a )
  \
  \ _a_ is the address of a secondary entry point of the Forth
  \ inner interpreter.  The code at _a_ pushes the DE and HL
  \ registers onto the stack and then continues at the address
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

[unneeded] << [unneeded] >> and ?(

  \ Credit:
  \
  \ Code adapted from Pygmy Forth 1.7 for DOS.
  \
  \ Original code by Frank Sergeant, for Pygmy Forth.
  \
  \ See the license at <http://pygmy.utoh.org/license.html>.

need c@+ need for need 16hex. need 8hex.

: << ( -- ca n ) here depth ;

  \ doc{
  \
  \ << ( -- ca +n )
  \
  \ Mark the start of a code zone to be dumped by `>>`.  _ca_
  \ is the current data-pointer and _+n_ is the current
  \ `depth`. Both of them are used by `>>`. See `>>` for a
  \ usage example.
  \
  \ Origin: Pygmy Forth.
  \
  \ }doc

: >> ( ca n -- )
  depth 1- <> #-258 ?throw
  cr dup 16hex. here over - for  c@+ 8hex.  step drop ; ?)

  \ doc{
  \
  \ >> ( ca +n -- )
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

  \ ===========================================================--
  \ Change log

  \ 2015, 2016: Unfinished conversion of Pygmy Forth's `<<` and
  \ `>>`.
  \
  \ 2017-05-09: Rename the file. Finish, test and document the
  \ code of `<<` and `>>`.
  \
  \ 2017-05-10: Add `pushhlde`.

  \ vim: filetype=soloforth

