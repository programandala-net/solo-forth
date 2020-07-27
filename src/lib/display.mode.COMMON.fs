  \ display.mode.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007280012
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words that are common to all display modes.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( form>xy >form form (at-xy columns rows set-mode-output )

unneeding form>xy ?( need columns need rows need */

: form>xy ( cols rows -- col row )
  xy swap >r rows */ r> swap >r columns */ r> ; ?)

  \ doc{
  \
  \ form>xy ( cols rows -- col row ) "form-to-x-y"
  \
  \ _col row_ is the new cursor position corresponding to a
  \ display mode whose `form` is _cols rows_. _col row_ are
  \ calculated with the values returned by `xy`, `columns` and
  \ `rows` in the current mode.
  \
  \ ``form>xy`` is a factor of `>form`.
  \
  \ }doc

unneeding >form ?( need form>xy need columns need rows need to

: >form ( cols rows -- )
  2dup form>xy at-xy to rows to columns ; ?)

  \ doc{
  \
  \ >form ( cols rows -- ) "to-form"
  \
  \ Adapt the cursor position of the current display mode to a
  \ display mode whose `form` is _cols rows_.
  \
  \ ``>form`` is used by the display modes, e.g. `mode-32` and
  \ `mode-64ao`.
  \
  \ NOTE: When ``>form`` is executed, the action of `at-xy`
  \ must be that of the new mode, but `xy`, `rows` and
  \ `columns` must still return the values of the current (old)
  \ mode.
  \
  \ }doc

unneeding form ?( need columns need rows

: form ( -- cols rows ) columns rows ; ?)

  \ doc{
  \
  \ form ( -- cols rows )
  \
  \ Number of `columns` and `rows` in the terminal in the
  \ current display mode (e.g. `mode-32`, `mode-64ao`).
  \
  \ Origin: Gforth.
  \
  \ }doc

unneeding (at-xy
?\ : (at-xy ( col row -- ) 22 emit swap emit emit ;

  \ doc{
  \
  \ (at-xy  ( col row -- ) "paren-at-x-y"
  \
  \ Set the cursor coordinates to column _col_ and row _row_,
  \ by displaying control character 22 followed by _col_ and
  \ _row_, as needed by some display modes, e.g. `mode-64ao`
  \ and `mode-42pw`.  The upper left corner is column zero, row
  \ zero.
  \
  \ ``(at-xy`` is a possible action of `at-xy`, which is a
  \ deferred word (see `defer`) configured by the current
  \ display mode.

  \ WARNING: The default `mode-32` expects _row_ right after
  \ control character 22, and then _col_, i.e in the order used
  \ by Sinclair BASIC. This will be fixed/unified in a future
  \ version of Solo Forth.
  \
  \ }doc

unneeding columns ?\ need cvalue 32 cvalue columns

  \ doc{
  \
  \ columns ( -- n )
  \
  \ Return the number of columns in the current screen mode.
  \ The default value is 32.
  \
  \ See: `rows`, last-column`, `column`.
  \
  \ }doc

unneeding rows ?\ need cvalue 24 cvalue rows

  \ doc{
  \
  \ rows ( -- n )
  \
  \ Return the number of rows in the current screen mode.  The
  \ default value is 24.
  \
  \ See: `columns`, `last-row`, `row`.
  \
  \ }doc

unneeding set-mode-output ?( need os-chans

: set-mode-output ( a -- )
  \ os-chans @ 2dup ! 2dup 5 + ! 15 + ! ; ?) \ XXX OLD
  os-chans @ 2dup ! 2dup 5 + ! 15 + ! ; ?)

  \ doc{
  \
  \ set-mode-output ( a -- )
  \
  \ Associate the output routine at _a_ to the system channels
  \ "K", "S" and "P".
  \
  \ }doc
  \ XXX TODO -- why also "P"?

( set-banked-mode-output )

  \ XXX UNDER DEVELOPMENT

need set-mode-output need >body

0 constant (output_

code (banked-mode-output ( -- )
  C5 c,  CD c, 0 ,
    \ push bc ; save Forth IP
    \ call output_routine ; to be patched
  here cell- ' (output_ >body !
    \ Store the address where the address of the output routine
    \ must be stored, into the constant `(output_`.
  C1 c,  DD c, 21 c, next ,  jpnext, end-code
    \ pop bc ; restore Forth IP
    \ ld ix,next ; restore IX, just in case
    \ _jp_next

: set-banked-mode-output ( a -- )
  (output_ !  \ patch `(banked-mode-output`
  ['] (banked-mode-output set-mode-output ;
  \ Associate the output routine at _a_ (which is in the code
  \ bank) to the system channels "K", "S" and "P", using and
  \ intermediate routine to page the code bank in and out.
  \ XXX OLD -- adapt to far memory or remove

  \ ===========================================================
  \ Change log

  \ 2016-05-07: Compact the blocks.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "COMMON", after the new convention.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Make `columns` and `rows` independent for
  \ `need` and document them.  Remove `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-16: Remove `set-font`, which is in the kernel.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-04: Update naming convention of Z80 routines, after
  \ the changes in the kernel.
  \
  \ 2017-04-16: Improve needing of `set-mode-output`.
  \
  \ 2017-04-19: Use `cvalue` instead of `value` for `columns`
  \ and `rows`.
  \
  \ 2017-04-21: Rename module after the new convention for
  \ display modes.  Add `(at-xy`, which was called `(at-xy)`
  \ and used by `mode-42`, `mode-64` and others.
  \
  \ 2017-05-15: Add `form>xy`, `>form` and `form`.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-09-08: Compact the code, saving one block.
  \
  \ 2017-12-09: Update with `need */`, since `*/` was moved to
  \ the library.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Update stack notation "x y" to "col row".
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.
  \
  \ 2020-02-27: Fix: update names that were renamed in
  \ 2018-01-24: "64o" (Owen) -> "64ao" (Andrew Owen).
  \
  \ 2020-05-04: Fix cross references: `mode-64` -> `mode-64ao`.
  \ `mode-42` -> `mode-42pw`.
  \
  \ 2020-07-28: Improve documentation of deferred words.

  \ vim: filetype=soloforth
