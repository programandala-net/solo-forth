  \ dos.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202101070152.
  \ See change log at the end of the file.

  \ ===========================================================
  \ Description

  \ Code common to any supported DOS.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018, 2020, 2021.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( drive ?set-drive ?drives )

unneeding drive ?\ : drive ( c1 -- c2 ) first-drive + ;

  \ XXX TODO Update documentation for NextZXOS.

  \ doc{
  \
  \ drive ( c1 -- c2 )
  \
  \ Convert drive number _c1_ (0 index) to actual drive
  \ identifier _c2_ (DOS dependent).
  \
  \ ``drive`` is used in order to make the code portable,
  \ abstracting the DOS drive identifiers.
  \
  \ Usage example:

  \ ----
  \ \ Set the second disk drive as default:
  \
  \ 2 set-drive       \ on G+DOS only
  \ 1 set-drive       \ on TR-DOS only
  \ 'B' set-drive     \ on +3DOS only
  \
  \ 1 drive set-drive \ on any DOS -- portable code
  \ ----

  \ See also: `set-drive`, `first-drive`, `max-drives`.
  \
  \ }doc

unneeding ?set-drive ?( need get-drive

: ?set-drive ( c -- ior )
  dup get-drive ?dup if nip nip nip exit then
  <> if set-drive exit then drop 0 ; ?)

  \ XXX TODO Update documentation for NextZXOS.

  \ doc{
  \
  \ ?set-drive ( c -- ior )
  \
  \ If drive _c_ is not equal to the current default drive,
  \ returned by `get-drive`, use `set-drive` to make _c_ the
  \ current default drive, returning I/O result code _ior_.
  \ Otherwise do nothing, and _ior_ is zero.
  \
  \ ``?set-drive`` is used by `(>drive-block`, in order to
  \ update the current default drive only when needed, i.e.
  \ when the desired block is not in the current default drive.
  \
  \ ifdef::plus3dos[]
  \
  \ That is especially useful on +3DOS, whose `set-drive` is
  \ slow because it has to do additional operations in order to
  \ make `transfer-sector` use the current default drive.
  \
  \ endif::[]
  \
  \ }doc

unneeding ?drives

?\ : ?drives ( n -- ) max-drives > #-287 ?throw ;

  \ doc{
  \
  \ ?drives ( n -- ) "question-drives"
  \
  \ If _n_ is greater than the maximum number of disk drives,
  \ `throw` an exception #-287 ("wrong number of drives").
  \
  \ See also: `set-block-drives`.  `?block-drive`, `?drive#`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-02-08: Start. First version of `set-block-drives` and
  \ related words.
  \
  \ 2017-02-09: Fix `(>drive-block`: `drive` is not needed,
  \ because `(block-drives` contains the actual identifiers,
  \ not the ordinal numbers. Fix and factor the check of drives
  \ number in `set-block-drives`.  Update `last-locatable` in
  \ `set-block-drives`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-04: Document `set-block-drives` and all related
  \ words.
  \
  \ 2017-03-06: Rename `block-drives` to `#block-drives`;
  \ rename `(block-drives` to `block-drives`.  Reorganize the
  \ code to make `get-block-drives` independent from
  \ `set-block-drives`. Remove `>block-drive`. Improve
  \ documentation. Remove the default configuration of
  \ `block-drives` that was set when `set-block-drives` was
  \ compiled.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-28: Fix typo.
  \
  \ 2017-04-21: Fix stack notation of `?drives`.
  \
  \ 2017-12-05: Fix and update stack notation.
  \
  \ 2018-01-03: Update `1literal` to `xliteral`.
  \
  \ 2018-02-04: Fix documentation. Improve documentation: add
  \ pronunciation to words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-15: Fix typo in the change log.
  \
  \ 2018-04-07: Improve documentation.
  \
  \ 2018-04-08: Add `?set-drive` in order to improve
  \ `(>drive-block`.
  \
  \ 2018-04-09: Improve documentation. Fix requirement of
  \ `?set-drive`. Fix `?set-drive`: replace the calculation,
  \ which failed with TR-DOS' drive 0, with an `if` structure.
  \
  \ 2018-04-16: Improve description of _ior_ notation.
  \
  \ 2018-06-05: Improve documentation.
  \
  \ 2018-06-16: Fix and improve documentation of
  \ `get-block-drives`.
  \
  \ 2018-07-21: Improve documentation, linking `throw`.
  \
  \ 2020-05-04: Fix documentation of `?block-drive`.
  \
  \ 2020-05-05: Improve documentation of `?set-drive`.
  \
  \ 2020-05-08: Move `b/sector` from the kernel.
  \
  \ 2020-05-24: Replace "hash" notation with "number sign".
  \
  \ 2020-05-26: Fix typo.
  \
  \ 2021-01-06: Update the description of this file.
  \
  \ 2021-01-07: Move to <dos.COMMON.block-drives.fs> all of the
  \ words that manage blocks directly in diskettes, shared by
  \ +3DOS, G+DOS and TR-DOS. Keep in <dos.COMMON.fs> only the
  \ code common to every supported DOS, which means NextZXOS.

  \ vim: filetype=soloforth
