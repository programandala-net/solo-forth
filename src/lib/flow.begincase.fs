  \ flow.begincase.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `begincase repeatcase`, an improved version of standard
  \ `case` that repeats like `begin again`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ Credit

  \ Original code by Ed:
  \ http://dxforth.netbay.com.au/cfsext.html

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( begincase )

need cs-push need cs-pop

: begincase ( -- )
  postpone case  postpone begin  cs-push
  ; immediate compile-only
  \ XXX TODO -- document the stack

: (repeatcase) ( -- )
  cs-pop  postpone again  postpone endcase ;
  \ XXX TODO -- document the stack

: repeatcase ( -- )
  postpone drop  postpone (repeatcase)
  ; immediate compile-only

  \ Usage example:
  \
  \ ----
  \ : test
  \   begincase
  \     cr ." press a key ('2' '4' '9' exits) : " key
  \     '2' of  ." ... 2 "  endof
  \     '4' of  ." ... 4 "  endof
  \     '9' of  ." ... 9 "  endof
  \       dup emit ."  try again"
  \   repeatcase ;
  \ ----

  \ ===========================================================
  \ Change log

  \ 2015-10-26: Start.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-04-27: Rename `nextcase` to `repeatcase`. Add
  \ `compile-only`.
  \
  \ 2016-08-05: Move the test to <meta.test.misc.fsb>, but keep
  \ a copy as usage example.

  \ vim: filetype=soloforth
