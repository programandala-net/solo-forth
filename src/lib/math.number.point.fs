  \ math.number.point.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to configure the charactes accepted as number point.

  \ ===========================================================
  \ Authors

  \ Wil Baden, published on Forth Dimensions (volume 20, number
  \ 3 page 26, 1998-10).

  \ Adapted by Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( classic-number-point? extended-number-point? )

unneeding classic-number-point? ?(

: classic-number-point? ( c -- f )
  dup ':' = swap ',' - 4 u< or ; ?)

  \ doc{
  \
  \ classic-number-point? ( c -- f )
  \
  \ Is character _c_ a classic number point?  Allowed points
  \ are: comma, hyphen, period, slash and colon.
  \
  \ ``classic-number-point?`` is an alternative action for the
  \ deferred word `number-point?`, which is used in `number?`,
  \ and whose default action is `standard-number-point?`.
  \
  \ See: `extended-number-point?`.
  \
  \ }doc

unneeding extended-number-point? ?(

: extended-number-point? ( c -- f )
  dup ':' = swap '+' - 5 u< or ; ?)

  \ doc{
  \
  \ extended-number-point? ( c -- f )
  \
  \ Is character _c_ an extended number point?  Allowed points
  \ are: plus sign, comma, hyphen, period, slash and colon,
  \ after _Forth Programmer's Handbook_.
  \
  \ ``extended-number-point?`` is an alternative action for the
  \ deferred word `number-point?`, which is used in `number?`,
  \ and whose default action is `standard-number-point?`.
  \
  \ See: `classic-number-point?`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2017-02-16: Fix typo in documentation.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-17: Fix index line. Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth

