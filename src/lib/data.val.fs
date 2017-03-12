  \ data.value.val.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ This module defines `val` and `toval`, which behave like
  \ standard `value` and `to` except `valto` doesn't parse: it
  \ changes the run-time behaviour of the value.
  \
  \ Also double-cell variant `2val` and `2toval` are included.
  \
  \ These words are 3-4 times slower than `value` and `2value`
  \ (from module "data.value.default.fsb"), but since they are
  \ non-parsing they may be useful in special cases.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( val 2val )

[unneeded] val ?(

variable (val)
: init-val ( -- )  ['] @ (val) ! ;  init-val
: val ( x "name" -- )
  create ,  does> ( -- ) ( pfa ) (val) perform  init-val ;
: toval    ( -- ) ['] ! (val) ! ; ?)

[unneeded] 2val ?(

variable (2val)
: init-2val ( -- )  ['] 2@ (2val) ! ;  init-2val
: 2val ( xd "name" -- )
  create 2,  does> ( -- ) ( pfa ) (2val) perform  init-2val ;
: 2toval    ( -- ) ['] 2! (2val) ! ; ?)

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

  \ vim: filetype=soloforth
