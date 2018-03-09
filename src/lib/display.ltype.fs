  \ display.ltype.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803091531
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tool to display left justified texts.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ltype )

need column need last-row need /first-name need columns
need 0exit need home? need seclusion

seclusion

variable #indented
  \ `#indented` = Indented characters in the current line.

-seclusion

variable #ltyped

  \ doc{
  \
  \ ltyped# ( -- a ) "l-typed-hash"
  \
  \ _a_ is the address of a cell containing the number of
  \ characters displayed by `ltype` on the current row.
  \
  \ See: `ltyped`.
  \
  \ }doc

: ltyped ( u -- ) #ltyped +! ;

  \ doc{
  \
  \ ltyped ( n -- ) "l-typed"
  \
  \ Update `ltyped#` with _n_ characters typed by `ltype`.
  \
  \ }doc

: indented+ ( u -- ) #indented +! ;

: (.word ( ca len -- ) dup ltyped type ;

: lemit ( c -- ) emit 1 ltyped ;  : lspace ( -- ) bl lemit ;

  \ doc{
  \
  \ lemit ( c -- ) "l-emit"
  \
  \ Display character _c_ as part of the left-justified displaying
  \ system.
  \
  \ See: `ltype`, `lspace`.
  \
  \ }doc

  \ doc{
  \
  \ lspace ( -- ) "l-space"
  \
  \ Display a space as part of the left-justified printing
  \ system.
  \
  \ See: `lemit`, `ltype`.
  \
  \ }doc

: no-ltyped ( -- ) #ltyped off #indented off ;

  \ doc{
  \
  \ no-ltyped ( -- ) "no-l-typed"
  \
  \ Set `ltyped#` and `#indented` to zero.
  \
  \ See: `ltyped`.
  \
  \ }doc

: lhome ( -- ) home no-ltyped ;

  \ doc{
  \
  \ lhome ( -- ) "l-home"
  \
  \ Move the cursor used by `ltype` and related words to its
  \ home position, at the top left (column 0, row 0).
  \
  \ }doc

: lpage ( -- ) cls lhome ;

  \ doc{
  \
  \ lpage ( -- ) "l-page"
  \
  \ Clear the display and init the cursor used by `ltype` and
  \ related words.
  \
  \ }doc

: lcr? ( -- f ) home? 0= column 0<> and ;

  \ doc{
  \
  \ lcr? ( -- f ) "l-c-r-question"
  \
  \ Is the cursor neither at the home position nor at the start of a
  \ line?  ``lcr?`` is part of the left-justified displaying system.
  \
  \ See: `lcr`, `ltype`.
  \
  \ }doc

defer (lcr) ( -- ) ' cr ' (lcr) defer! -->

  \ doc{
  \
  \ (lcr) ( -- ) "paren-l-c-r"
  \
  \ A deferred word whose default action is `cr`.  This is the
  \ actual carriage return done by `lcr`, before updating the
  \ data of the left-justified displaying system.  ``(lcr)`` is
  \ a hook for the application, for special cases.
  \
  \ See: `ltype`.
  \
  \ }doc

( ltype )

: lcr ( -- ) lcr? if (lcr) then no-ltyped ;

  \ doc{
  \
  \ lcr ( -- ) "l-c-r"
  \
  \ If the cursor is neither at the home position nor at the
  \ start of a line, move it to the next row. ``lcr`` is part
  \ of the left-justified displaying system. 
  \
  \ See: `lcr?`, `(lcr)`, `ltype`.
  \
  \ }doc

create lwidth columns c,

  \ doc{
  \
  \ lwidth ( -- ca ) "l-width"
  \
  \ A byte variable containing the text width in columns used
  \ by `ltype` and related words. Its default value is
  \ `columns`, ie. the current width of the screen.
  \
  \ }doc

+seclusion

: previous-word? ( -- f ) #ltyped @ #indented @ > ;
  \ Has a word been displayed before in the current line?

: ?space ( -- ) previous-word? if bl lemit then ;
  \ If a word was displayed before in the current line, display
  \ a space as separator.

: unfit? ( len -- f ) 1+ #ltyped @ + lwidth c@ > ;
  \ Is word length _len_ too long to be displayed in the
  \ current line?

: .word ( ca len -- )
  dup unfit? if lcr else ?space then (.word ;
  \ Display word _ca len_ left-justified from the current
  \ cursor position.

: (ltype-indentation ( u -- )
  dup spaces dup indented+ ltyped ;

-seclusion

: ltype-indentation ( u -- ) ?dup 0exit (ltype-indentation ;

: ltype ( ca len --)
  begin dup while /first-name .word repeat 2drop ;

  \ doc{
  \
  \ ltype ( ca len -- ) "l-type"
  \
  \ Display character string _ca len_ left-justified from the
  \ current cursor position.
  \
  \ See: `lwidth`.
  \
  \ }doc

end-seclusion -->

( ltype )

  \ ===========================================================
  \ Debugging test

  \ XXX TMP --

need n>str

: t ( n -- )
  0 ?do
    \ lrow#  @ n>str ltype
    \ lrows# @ n>str ltype
    s" En un lugar de La Mancha." ltype
  loop ;

  \ ===========================================================
  \ Change log

  \ 2017-02-03: Compact the code, saving two blocks.
  \
  \ 2017-04-20: Review. Modify layout.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-05-08: Rename "print" to "ltype" in all words,
  \ including the filename of the module.
  \
  \ 2017-08-17: Remove old useless code, after the recent
  \ changes in Galope's `ltype`.
  \
  \ 2017-09-08: Reduce factoring of `first-word`, rename it
  \ `/first-name` and move it to <strings.MISC.fs>.  Rename all
  \ "ltype-cr" to "lcr".  Fix initializacion of `(lcr`.
  \ Complete the requirements. Use `home?`.
  \
  \ 2017-12-04: Compact the code, saving one block. Use
  \ `seclusion`. Update names after Galope's `ltype` module.
  \ Improve documentation.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-03-09: Make `lwidth` a byte variable. Fix `unfit?`.

  \ vim: filetype=soloforth
