  \ data.astack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `astack`, an implementation of an extra stack.

  \ ===========================================================
  \ Authors

  \ Copyright 2004,2007 J.L. Bezemer

  \ Modified and adapted to Solo Forth by Marcos Cruz
  \ (programandala.net), 2018.

  \ ===========================================================
  \ Credit

  \ Copied and modified from:
  \   4tH library - file <stack.4th>
  \   http://www.xs4all.nl/~thebeez/4tH

  \ ===========================================================
  \ License

  \ You can redistribute this file and/or modify it under
  \ the terms of the GNU General Public License.

( astack adepth )

unneeding astack ?(

: astack ( a -- ) dup ! ;

  \ doc{
  \
  \ astack ( a -- )
  \
  \ Init extra stack _a_. The extra stack will grow towards
  \ high memory and the required memory must be already
  \ reserved.  No check is done by ``astack`` or the other
  \ words used to manipulate the extra stack.
  \
  \ Usage example:

  \ ----
  \ create my-stack 10 cells allot
  \ my-stack astack
  \ 100 my-stack >a
  \ my-stack adepth .
  \ my-stack a@ .
  \ my-stack a> .
  \ my-stack adepth .
  \ ----

  \ See: `>a`, `a@`, `a>`, `adepth`, `xstack`.
  \
  \ }doc

: a@ ( a -- x ) @ @ ;

  \ doc{
  \
  \ a@ ( a -- x )
  \
  \ Copy _x_ from the `astack` _a_ to the data stack.
  \
  \ See: `a>`, `>a`.
  \
  \ }doc

: >a ( x a -- ) cell over +! @ ! ;

  \ doc{
  \
  \ >a ( x a -- )
  \
  \ Move _x_ to the extra stack _a_ defined with `astack`.
  \
  \ See: `a>`, `a@`.
  \
  \ }doc

: a> ( a -- x ) dup a@ [ cell negate ] literal rot +! ;

  \ doc{
  \
  \ a> ( a -- x )
  \
  \ Move _x_ from the extra stack _a_ defined with `astack` to
  \ the data stack.
  \
  \ See: `>a`, `a@`.
  \
  \ }doc

?)

unneeding adepth ?( need cell/

: adepth ( a -- n ) dup @ swap - cell/ ; ?)

  \ doc{
  \
  \ adepth ( a -- n )
  \
  \ Return size _n_ in cells of an `astack` _a_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2018-02-07: Start.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
