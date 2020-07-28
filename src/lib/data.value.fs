  \ data.value.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282109
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Standard `value`, `2value` and `to`; non-standard `cvalue`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( cvalue value 2value to )

unneeding cvalue ?( need to need ;code

: cvalue ( c "name" -- )
  create 0 c, c, ;code 23 c, ' c@ 1+ jp, end-code ?)
                   \ inc hl
                   \ jp c_fetch.hl

  \ doc{
  \
  \ cvalue ( c "name" -- ) "c-value"
  \
  \ Create a definition _name_ with initial value _c_. When
  \ _name_ is later executed, _c_ will be placed on the stack.
  \ `to` can be used to assign a new value to _name_.
  \
  \ See also: `value`, `2value`, `cconstant`, `cvariable`, `cval`.
  \
  \ }doc

unneeding value ?( need to need ;code

: value ( n "name" -- )
  create 1 c, , ;code 23 c, ' @ 1+ jp, end-code ?)
                  \ inc hl
                  \ jp fetch.hl

  \ doc{
  \
  \ value ( x "name" -- )
  \
  \ Create a definition _name_ with initial value _x_. When
  \ _name_ is later executed, _x_ will be placed on the stack.
  \ `to` can be used to assign a new value to _name_.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `cvalue`, `2value`, `constant`, `variable`, `val`.
  \
  \ }doc

unneeding 2value ?( need to need ;code

: 2value ( x1 x2 "name" -- )
  create 2 c, 2, ;code 23 c, ' 2@ 1+ jp, end-code ?)
                   \ inc hl
                   \ jp two_fetch.hl

  \ doc{
  \
  \ 2value ( x1 x2 "name" -- ) "two-value"
  \
  \ Create a definition _name_ with initial value _x1 x2_. When
  \ _name_ is later executed, _x1 x2_ will be placed on the
  \ stack.  `to` can be used to assign a new value to _name_.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `cvalue`, `value`, `2constant`, `2variable`, `2val`.
  \
  \ }doc

unneeding to ?( need >body need array>

create to> ' c! , ' ! , ' 2! ,

: to
  \ Interpretation: ( i*x "name" -- )
  \ Compilation:    ( "name" -- )
  \ Run-time        ( i*x -- )
  ' >body dup 1+ swap c@ to> array>
  compiling? if swap postpone literal @ compile, exit then
             perform ; immediate ?)

  \ doc{
  \
  \ to
  \   Interpretation: ( i*x "name" -- )
  \   Compilation:    ( "name" -- )
  \   Run-time:       ( i*x -- )

  \ ``to`` is an `immediate` word.
  \
  \ Interpretation:
  \
  \ Parse _name_, which is a word created by `cvalue`, `value`
  \ or `2value`, and make _i*x_ its value.
  \
  \ Compilation:
  \
  \ Parse _name_, which is a word created by `cvalue`, `value`
  \ or `2value`, and append the execution execution semantics
  \ given below to the current definition.
  \
  \ Run-time:
  \
  \ Make _i*x_ the value of _name_.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See also: `!>`, `c!>`, `2!>`, `toval`, `ctoval`, `2toval`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09-25: Benchmark.
  \
  \ 2016-05-10: Improve `2value`.
  \
  \ 2016-05-11: Document.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-30: Improve documentation.
  \
  \ 2017-04-08: Improve documentation. Make `value` and
  \ `2value` accessible to `need`. Rewrite `to` with an
  \ execution table (11 bytes saved). Add `cvalue`.
  \
  \ 2017-04-09: Improve documentation.
  \
  \ 2017-04-16: Improve documentation.
  \
  \ 2017-05-15: Improve documentation.
  \
  \ 2017-12-11: Fix and improve needing of `to`.
  \
  \ 2018-01-20: Improve documentation.
  \
  \ 2018-01-24: Rewrite the run-time part of `cvalue`, `value`
  \ and `2value` in Z80.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton. Improve documentation.
  \
  \ 2018-03-11: Fix requirement of `2value`.

  \ vim: filetype=soloforth
