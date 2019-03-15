  \ data.begin-stringtable.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201903151508
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `begin-stringtable` and `end-stringtable`.

  \ ===========================================================
  \ Author

  \ Copyright (C) 2007 Dick van Oudheusden
  \
  \ Marcos Cruz (programandala.net) adapted the code to Solo
  \ Forth, 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ This library module is free software; you can redistribute
  \ it and/or modify it under the terms of the GNU Lesser
  \ General Public License as published by the Free Software
  \ Foundation; either version 3 of the License, or (at your
  \ option) any later version.
  \
  \ This library is distributed in the hope that it will be
  \ useful, but WITHOUT ANY WARRANTY; without even the implied
  \ warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
  \ PURPOSE.  See the GNU Lesser General Public License for
  \ more details.
  \
  \ You should have received a copy of the GNU Lesser General
  \ Public License along with this library; if not, see
  \ <http://www.gnu.org/licenses/lgpl.html>.

( begin-stringtable end-stringtable )

  \ Credit:
  \
  \ Code adapted from Forth Foundation Library (module "stt").

need array>

: begin-stringtable ( "name" -- a1 a2 )
  create here ( a1 ) cell allot here ( a1 a2 )
  does> ( n -- ca len )
    \ Return the nth string.
    ( n dfa ) @ array> @ count ;

  \ doc{
  \
  \ begin-stringtable ( "name" -- a1 a2 )
  \
  \ Start a named stringtable definition "name", returning _a1_
  \ (containing the address of the strings index) and _a2_ (the
  \ address of the compiled strings), to be consumed by
  \ `end-stringtable`.
  \
  \ Usage example:

  \ ----
  \ begin-stringtable esperanto-number
  \   s" nulo" s,
  \   s" unu"  s,
  \   s" du"   s,
  \   s" tri"  s,
  \ end-stringtable
  \
  \ 0 esperanto-number type
  \ 3 esperanto-number type
  \ ----

  \ See: `sconstants`.
  \
  \ }doc

: end-stringtable ( a1 a2 -- )
  here rot !   \ set the index
  here swap ( a3 a2 )
  begin 2dup <> while
    dup ,   \ store the start of the strings in the index
    count chars +  \ move to the next string
  repeat  2drop ;

  \ doc{
  \
  \ end-stringtable ( a1 a2 -- )
  \
  \ End a named stringtable, consuming _a1_ (containing the
  \ address of the strings index) and _a2_ (the address of the
  \ compiled strings), which were left by `begin-stringtable`.
  \ Create the strings index by traversing the compiled strings
  \ and update its address in _a1_.
	\
  \
  \ See `begin-stringtable` for a usage example.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-02-07: Improve documentation. Add the GNU Lesser
  \ General Public License v3.0.
  \
  \ 2018-05-01: Fix filename in the file header.
	\
	\ 2019-03-15: Improve documentation.

  \ vim: filetype=soloforth
