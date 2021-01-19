  \ prog.editor.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202101192013.
  \ See change log at the end of the file.

  \ ===========================================================
  \ Description

  \ This module contains code common to any editor.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( r# top editor )

unneeding r# unneeding top and ?(

variable r#

  \ doc{
  \
  \ r# ( -- a ) "r-slash"
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ location of the editing cursor, an offset from the top of
  \ the current block. Its default value is zero.
  \
  \ ``r#`` is used by `specforth-editor` and `gforth-editor`.
  \
  \ Origin: fig-Forth's user variable ``r#``.
  \
  \ See also: `top`.
  \
  \ }doc

: top ( -- ) r# off ; top ?)

  \ doc{
  \
  \ top ( -- )
  \
  \ Position the editing cursor at the top of the block, by
  \ setting `r#` to zero.
  \
  \ ``top`` is used by `specforth-editor` and `gforth-editor`.
  \
  \ }doc

unneeding editor ?\ defer editor

  \ doc{
  \
  \ editor ( -- )
  \
  \ Replace the first entry in the search order with the word
  \ list associated to the block editor.
  \
  \ ``editor`` is a deferred word (see `defer`). Its action can
  \ be `gforth-editor` or `specforth-editor`. When any of these
  \ editors is loaded, ``editor`` is updated accordingly.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-22: Start, with code extracted from
  \ <editor.specforth.fsb> and <editor.blocked.fsb>.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "COMMON", after the new convention.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-02-27: Update source style (remove double spaces).
  \
  \ 2018-06-04: Link `variable` in documentation.
  \
  \ 2020-05-03: Fix/improve documentation. Add `editor`.
  \
  \ 2020-05-05: Improve documentation of `editor`.
  \
  \ 2020-05-13: Improve documentation of `editor`.
  \
  \ 2020-07-28: Improve documentation of deferred words.
  \
  \ 2021-01-19: Fix needing of `r#` and `top`.

  \ vim: filetype=soloforth
