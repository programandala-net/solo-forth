  \ data.estack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803062223
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `estack`, an implementation of an extra stack.

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

( estack edepth )

unneeding estack ?(

: estack ( a -- ) dup ! ;

  \ doc{
  \
  \ estack ( a -- ) "e-stack"
  \
  \ Init extra stack _a_. The extra stack will grow towards
  \ high memory and the required memory must be already
  \ reserved.  No check is done by ``estack`` or the other
  \ words used to manipulate the extra stack.
  \
  \ Usage example:

  \ ----
  \ create my-stack 10 cells allot
  \ my-stack estack
  \ 100 my-stack >e
  \ my-stack edepth .
  \ my-stack e@ .
  \ my-stack e> .
  \ my-stack edepth .
  \ ----

  \ See: `>e`, `e@`, `e>`, `edepth`, `xstack`.
  \
  \ }doc

: e@ ( a -- x ) @ @ ;

  \ doc{
  \
  \ e@ ( a -- x ) "e-fetch"
  \
  \ Copy _x_ from the `estack` _a_ to the data stack.
  \
  \ See: `e>`, `>e`.
  \
  \ }doc

: >e ( x a -- ) cell over +! @ ! ;

  \ doc{
  \
  \ >e ( x a -- ) "to-e"
  \
  \ Move _x_ to the extra stack _a_ defined with `estack`.
  \
  \ See: `e>`, `e@`.
  \
  \ }doc

: e> ( a -- x ) dup e@ [ cell negate ] literal rot +! ;

  \ doc{
  \
  \ e> ( a -- x ) "e-from"
  \
  \ Move _x_ from the extra stack _a_ defined with `estack` to
  \ the data stack.
  \
  \ See: `>e`, `e@`.
  \
  \ }doc

?)

unneeding edepth ?( need cell/

: edepth ( a -- n ) dup @ swap - cell/ ; ?)

  \ doc{
  \
  \ edepth ( a -- n ) "e-depth"
  \
  \ Return size _n_ in cells of an `estack` _a_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2018-02-07: Start.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-06: Rename `astack` `estack`; rename "a" "e". This
  \ fixes name clashing with address register's `a@`. Add
  \ pronunciation.

  \ vim: filetype=soloforth
