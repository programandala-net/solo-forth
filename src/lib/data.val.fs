  \ data.val.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ This module provides `val` and `toval`, which behave like
  \ standard `value` and `to` except `valto` doesn't parse: it
  \ changes the run-time behaviour of the words created by
  \ `val`.
  \
  \ Also double-cell variant `2val` and `2toval` and character
  \ variant `cval` and `ctoval` are provided.
  \
  \ These words are 3-4 times slower than using `constant` and
  \ `!>`, or `2constant` and `2!>` but since they are
  \ non-parsing they may be useful in special cases.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( val 2val cval )

unneeding val ?(  variable (val)

: init-val ( -- ) ['] @ (val) ! ; init-val

  \ doc{
  \
  \ init-val  ( -- )
  \
  \ Init the default behaviour of words created by `val`: Make
  \ them return their content.
  \
  \ ``init-val`` is a factor of `val`.
  \
  \ }doc

: val ( x "name" -- )
  create , does> ( -- ) ( dfa ) (val) perform init-val ;

  \ doc{
  \
  \ val ( x "name" -- )
  \
  \ Create a definition for _name_ that will place _x_ on the
  \ stack (unless `toval` is used first) and then will execute
  \ `init-val`.
  \
  \ ``val`` is an alternative to the standard `value`.
  \
  \ See: `cval`, `2val`, `variable`, `constant`.
  \
  \ }doc

: toval ( -- ) ['] ! (val) ! ; ?)

  \ doc{
  \
  \ toval ( -- )
  \
  \ Change the default behaviour of words created by `val`:
  \ make them store a new value instead of returning its actual
  \ one.
  \
  \ ``toval`` and `val` are a non-parsing alternative to the
  \ standard `to` and `value`.
  \
  \ See: `ctoval`, `2toval`.
  \
  \ }doc

unneeding 2val ?(  variable (2val)

: init-2val ( -- ) ['] 2@ (2val) ! ; init-2val

  \ doc{
  \
  \ init-2val  ( -- )
  \
  \ Init the default behaviour of words created by `2val`: Make
  \ them return their content.
  \
  \ ``init-2val`` is a factor of `2val`.
  \
  \ }doc

: 2val ( xd "name" -- )
  create 2, does> ( -- ) ( dfa ) (2val) perform init-2val ;

  \ doc{
  \
  \ 2val ( x1 x2 "name" -- )
  \
  \ Create a definition for _name_ that will place _x1 x2_ on
  \ the stack (unless `2toval` is used first) and then will
  \ execute `init-2val`.
  \
  \ ``2val`` is an alternative to the standard `2value`.
  \
  \ See: `val`, `cval`, `2variable`, `2constant`.
  \
  \ }doc

: 2toval ( -- ) ['] 2! (2val) ! ; ?)

  \ doc{
  \
  \ 2toval ( -- )
  \
  \ Change the default behaviour of words created by `2val`:
  \ make them store a new value instead of returning its actual
  \ one.
  \
  \ ``2toval`` and `2val` are a non-parsing alternative to the
  \ standard `to` and `2value`.
  \
  \ See: `toval`, `ctoval`.
  \
  \ }doc

unneeding cval ?(  variable (cval)

: init-cval ( -- ) ['] c@ (cval) ! ; init-cval

  \ doc{
  \
  \ init-cval  ( -- )
  \
  \ Init the default behaviour of words created by `cval`: Make
  \ them return their content.
  \
  \ ``init-cval`` is a factor of `cval`.
  \
  \ }doc

: cval ( xd "name" -- )
  create c, does> ( -- ) ( dfa ) (cval) perform init-cval ;

  \ doc{
  \
  \ cval ( c "name" -- )
  \
  \ Create a definition for _name_ that will place _c_ on
  \ the stack (unless `ctoval` is used first) and then will
  \ execute `init-cval`.
  \
  \ See: `val`, `2val`, `cvariable`, `cconstant`.
  \
  \ }doc

: ctoval ( -- ) ['] c! (cval) ! ; ?)

  \ doc{
  \
  \ ctoval ( -- )
  \
  \ Change the default behaviour of words created by `cval`:
  \ make them store a new value instead of returning its actual
  \ one.
  \
  \ See: `toval`, `2toval`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09-08: First versions of `value` with non-standard
  \ non-parsing `to`, inspired by lina.
  \
  \ 2015-09-25: Benchmarked. `perform` and flag versions are
  \ faster than the `defer` version.
  \
  \ 2016-05-10: Fix typo. Improve documentation.
  \
  \ 2016-05-11: Rename `value` to `val` and `to` to `toval`.
  \ Factor the initialization. Add a double-cell version.
  \
  \ 2017-03-30: Add `cval` and `ctoval`. Document all words.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
