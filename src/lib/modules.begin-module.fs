  \ modules.begin-module.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Implementation of named and unnamed modules.
  \
  \ Modules hide the internal implementation and leave visible
  \ the words of the outer interface.
  \
  \ This implementation supports any number of groups of
  \ private and public words, in any order. Beside, named
  \ modules make it possible to use the private words of the
  \ module, if needed.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ Credit

  \ Code adapted and modified from Galope.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-11-09: First version, adapted from Galope.
  \ 2016-12-06: Comment name clashes with `package`.

( begin-module: begin-module public private end-module )

  \ XXX TODO -- combine with `package`

need get-order need wordlist

  \ Inner words

get-order get-current

wordlist dup set-current  >order

variable current-wid  variable module-wid

: (begin-module) ( -- wid )
  get-current current-wid !
  wordlist dup module-wid ! dup >order ;

set-current

  \ Interface words

: public ( -- ) current-wid @ set-current ;

  \ XXX FIXME -- name clash with `package`

  \ doc{
  \
  \ public ( -- )
  \
  \ Public definitions of a module follow.
  \ See `begin-module:` for a usage example.
  \
  \ }doc

: private ( -- ) module-wid @ set-current ;

  \ XXX FIXME -- name clash with `package`

  \ doc{
  \
  \ private ( -- )
  \
  \ Private definitions of a module follow.
  \ See `begin-module:` for a usage example.
  \
  \ }doc


: begin-module: ( "name" -- )
  (begin-module) constant private ;

  \ doc{
  \
  \ begin-module: ( "name" -- )
  \
  \ Start a named module _name_.
  \ Private definitions follow.
  \
  \ Modules hide the internal implementation and leave visible
  \ the words of the outer interface.
  \
  \ Usage example:

  \ ----
  \ begin-module: my_module
  \   \ Inner/helper words.
  \ public
  \   \ Interface words,
  \   \ compiled in the outer vocabulary,
  \   \ thus seen from the extern.
  \ private
  \   \ Inner/helper words again.
  \ public
  \   \ Interface words again. And so on.
  \ end-module
  \ ----

  \ The private words can be found using the module name,
  \ which returns the _wid_ of its word list.

  \ As an alternative, the word `begin-module` starts an
  \ unnamed module.
  \
  \ }doc

: begin-module ( -- ) (begin-module) drop private ;

  \ doc{
  \
  \ begin-module ( -- )
  \
  \ Start an anonymous module.
  \ Private definitions follow.
  \ See `begin-module:` for a usage example.
  \
  \ }doc

: end-module ( -- ) public previous ;

  \ doc{
  \
  \ end-module ( -- )
  \
  \ End a module.
  \ See `begin-module:` for a usage example.
  \
  \ }doc

set-order

  \ vim: filetype=soloforth
