  \ os.variables.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803232354
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Constants for the OS variables.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( os-chars os-chans os-flags2 os-seed os-frames os-udg )

unneeding os-chars ?\ #23606 constant os-chars

  \ doc{
  \
  \ os-chars ( -- a ) "o-s-chars"
  \
  \ A constant that returns the address of system variable
  \ CHARS, which holds the bitmap address of character 0 of the
  \ current font (actual characters 32..127). By default this
  \ system variables holds ROM address 15360 ($3C00).
  \
  \ See: `set-font`, `get-font`, `rom-font`, `os-udg`.
  \
  \ }doc

unneeding os-chans ?\ #23631 constant os-chans

  \ doc{
  \
  \ os-chans ( -- a ) "o-s-chans"
  \
  \ A constant that returns the address _a_ of the system
  \ variable CHANS, which holds the address of the channel data
  \ table. Each element of the table has the following
  \ structure:

  \ |===
  \ | Offset (bytes) | Content
  \
  \ | +0             | Address of the channel output routine
  \ | +2             | Address of the channel input routine
  \ | +4             | Channel identifier character
  \ |===

  \ The default contents of the channel data table are the
  \ following:

  \ |===
  \ | Offset (bytes) | Content
  \
  \ | +0             | $09F4 (print-out)
  \ | +2             | $10A8 (key-input)
  \ | +4             | 'K'
  \ | +5             | $09F4 (print-out)
  \ | +7             | $15C4 (report-j)
  \ | +9             | 'S'
  \ | +10            | $08F1 (add-char)
  \ | +12            | $15C4 (report-j)
  \ | +14            | 'R'
  \ | +15            | $09F4 (print-out)
  \ | +17            | $15C4 (report-j)
  \ | +19            | 'P'
  \ |===

  \ NOTE: The elements of the channel data table are pointed
  \ from `os-strms` by 1-indexed byte offsets, i.e. $0001
  \ points to the first element of the channel data table,
  \ channel 'K'.
  \
  \ See: `.os-chans`.
  \
  \ }doc

unneeding os-flags2 ?\ #23658 constant os-flags2

  \ doc{
  \
  \ os-flags2 ( -- ca ) "o-s-flags-two"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable FLAGS2, which holds several flags.
  \
  \ See: `capslock`.
  \
  \ }doc

unneeding os-seed ?\ #23670 constant os-seed

  \ doc{
  \
  \ os-seed ( -- a ) "o-s-seed"
  \
  \ A constant that returns the address _a_ of system variable
  \ SEED, which holds the seed of the BASIC random number
  \ generator.
  \
  \ }doc

unneeding os-frames ?\ #23672 constant os-frames

  \ doc{
  \
  \ os-frames ( -- a ) "o-s-frames"
  \
  \ A constant that returns the address _a_ of the 24-bit
  \ system variable FRAMES (least significant byte first),
  \ containing the counter of frames, which is incremented
  \ every 20 ms by the interrupt routine of the OS. This
  \ counter is returned by `ticks` and used by its related
  \ words.
  \
  \ See: `set-ticks`, `reset-ticks`,
  \ `ticks-pause`, `?ticks-pause`.
  \
  \ }doc

unneeding os-udg ?\ #23675 constant os-udg

  \ doc{
  \
  \ os-udg ( -- a ) "o-s-u-d-g"
  \
  \ A constant that returns the address _a_ of system variable
  \ UDG, which holds the address of the first character bitmap
  \ of the current User Defined Graphics set (characters
  \ 128..255 or 0..255, depending on the words used to access
  \ them).
  \
  \ See: `set-udg`, `get-udg`, `os-chars`.
  \
  \ }doc

( os-coords os-coordx os-coordy os-strms os-prog os-ramtop )

unneeding os-coords ?\ #23677 constant os-coords

  \ doc{
  \
  \ os-coords ( -- a ) "o-s-coords"
  \
  \ A constant that returns the address _a_ of 2-byte system
  \ variable COORDS which holds the graphic coordinates of the
  \ last point plotted.
  \
  \ See: `set-pixel`, `plot`, `os-coordx`, `os-coordy`.
  \
  \ }doc

unneeding os-coordx ?\ #23677 constant os-coordx

  \ doc{
  \
  \ os-coordx ( -- ca ) "o-s-coord-x"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable COORDX which holds the graphic x coordinate of the
  \ last point plotted.
  \
  \ See: `set-pixel`, `plot`, `os-coords`, `os-coordy`.
  \
  \ }doc

unneeding os-strms ?\ #23568 constant os-strms

  \ doc{
  \
  \ os-strms ( -- a ) "o-s-streams"
  \
  \ A constant that returns the address _a_ of a 38-byte
  \ (19-cell) system variable STRMS which holds one cell per
  \ stream, containing the address of the channel attached to
  \ it, as follows:

  \ |===
  \ | Offset (cells) | Stream | Content
  \
  \ | +0             | -3     | $0001 (offset to channel 'K')
  \ | +1             | -2     | $0006 (offset to channel 'S')
  \ | +2             | -1     | $000B (offset to channel 'R')
  \ | +3             | 0      | $0001 (offset to channel 'K')
  \ | +4             | 1      | $0001 (offset to channel 'K')
  \ | +5             | 2      | $0006 (offset to channel 'S')
  \ | +6             | 3      | $0010 (offset to channel 'P')
  \ | +7..+18        | 4..15  | $0000..$0000 (not attached)
  \ |===

  \ NOTE: The contents are 1-index offsets from the address
  \ `os-chans`.  When the content of a stream cell is zero, the
  \ stream is not attached to a channel.
  \
  \ See: `.os-strms`.
  \
  \ }doc

unneeding os-coordy ?\ #23678 constant os-coordy

  \ doc{
  \
  \ os-coordy ( -- ca ) "o-s-coord-y"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable COORDY which holds the graphic y coordinate of the
  \ last point plotted.
  \
  \ See: `set-pixel`, `plot`, `os-coords`, `os-coordx`.
  \
  \ }doc

unneeding os-prog ?\ #23635 constant os-prog

  \ doc{
  \
  \ os-prog ( -- a ) "o-s-prog"
  \
  \ A constant that returns the address _a_ of 2-byte system
  \ variable PROG which holds the address of the BASIC program.
  \
  \ See: `os-stkend`, `os-ramtop`, `os-chans`.
  \
  \ }doc

unneeding os-ramtop ?\ #23730 constant os-ramtop

  \ doc{
  \
  \ os-ramtop ( -- a ) "o-s-ram-top"
  \
  \ A constant that returns the address _a_ of 2-byte system
  \ variable RAMTOP which holds the address of the last byte of
  \ BASIC system area.
  \
  \ See: `os-stkend`, `os-prog`, `os-chans`.
  \
  \ }doc

( os-attr-p os-mask-p os-attr-t os-mask-t os-p-flag os-stkend )

unneeding os-attr-p ?\ #23693 constant os-attr-p

  \ doc{
  \
  \ os-attr-p ( -- ca ) "o-s-attribute-p"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable ATTR_P, which holds the current permanent color
  \ attribute, as set up by color statements.
  \
  \ See: `os-attr-t`, `as-mask-p`.
  \
  \ }doc

unneeding os-mask-p ?\ #23694 constant os-mask-p

  \ doc{
  \
  \ os-mask-p ( -- ca ) "o-s-mask-p"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable MASK_P, which holds the permanent color attribute
  \ mask, used for transparent colors, etc. Any bit that is 1
  \ shows that the corresponding attribute bit is taken not
  \ from `os-attr-p` but from what is already on the screen.
  \
  \ See: `os-attr-p`, `os-mask-t`.
  \
  \ }doc

unneeding os-attr-t ?\ #23695 constant os-attr-t

  \ doc{
  \
  \ os-attr-t ( -- ca ) "o-s-attribute-t"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable ATTR_T, which holds the current temporary color
  \ attribute, as set up by color statements.
  \
  \ See: `os-attr-p`, `os-mask-t`.
  \
  \ }doc

unneeding os-mask-t ?\ #23696 constant os-mask-t

  \ doc{
  \
  \ os-mask-t ( -- ca ) "o-s-mask-t"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable MASK_T, which holds the temporary color attribute
  \ mask, used for transparent colors, etc. Any bit that is 1
  \ shows that the corresponding attribute bit is taken not
  \ from `os-attr-t` but from what is already on the screen.
  \
  \ See: `os-attr-t`, `os-mask-p`.
  \
  \ }doc

unneeding os-p-flag ?\ #23697 constant os-mask-t

  \ doc{
  \
  \ os-p-flag ( -- ca ) "o-s-p-flag"
  \
  \ A constant that returns the address _ca_ of 1-byte system
  \ variable P_FLAG, which holds some flags related to
  \ printing.
  \
  \ }doc

unneeding os-stkend ?\ #23653 constant os-stkend

  \ doc{
  \
  \ os-stkend ( -- a ) "o-s-stack-end"
  \
  \ A constant that returns the address _a_ of 2-byte system
  \ variable STKEND which holds the address of the start of
  \ spare space of BASIC system area.
  \
  \ See: `os-prog`, `os-chans`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-23: Fix: graphic coordinates variables were not
  \ included in the block title.
  \
  \ 2016-05-01: Add `os-attr-p`, `os-mask-p`, `os-attr-t`,
  \ `os-mask-t`
  \
  \ 2017-01-10: Document all words.
  \
  \ 2017-01-18: Improve documentation of `os-frames`.
  \
  \ 2017-01-31: Add the decimal prefix to addresses, in case
  \ `base` is not decimal.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-11-28: Update: replace "frames" words with "ticks".
  \ Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-03-23: Add `os-strms`. Improve documentation of
  \ `os-chans`. Fix and update documentation. Rename the file.
  \ Add `os-prog`, `os-ramtop`, `os-stkend`.

  \ vim: filetype=soloforth
