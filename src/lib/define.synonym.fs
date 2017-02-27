  \ define.synonym.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ An implementation of Forth-2012 `synonym`.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015-10-25: First version of `synonym`, using `create
  \ does>`.
  \
  \ 2015-12-23: New improved version, using `alias`. Keep the
  \ first version, just in case.
  \
  \ 2016-04-18: Removed the old first version.
  \
  \ 2016-04-24: Add `need nextname`, because `nextname` has been
  \ moved from the kernel to the library.
  \
  \ 2016-11-18: Improve documentation.
  \
  \ 2017-02-17: Update cross references.

( synonym )

need alias need nextname

: synonym ( "newname" "oldname" -- )
  parse-name nextname ' dup >r alias
  r> >name dup immediate?     if  immediate     then
               compile-only?  if  compile-only  then ;

  \ doc{
  \
  \ synonym ( "newname" "oldname" -- )
  \
  \ Create a definition for _newname_ with the the semantics
  \ defined below. _newname_ may be the same as _oldname_; when
  \ looking up _oldname_, _newname_ shall not be found.
  \
  \ newname ( Interpretation: i*x -- j*x )
  \
  \ Perform the interpretation semantics of _oldname_.
  \
  \ newname ( Compilation: i*x -- j*x )
  \
  \ Perform the compilation semantics of _oldname_.
  \
  \ Synonyms have the execution token of the old word and,
  \ contrary to aliases created by `alias`, they also inherit
  \ its attributes (immediate and compile-only).
  \
  \ See also: `alias`.
  \
  \ Origin: Forth-2012 (TOOLS EXT).
  \
  \ }doc

  \ vim: filetype=soloforth
