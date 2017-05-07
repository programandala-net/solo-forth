  \ display.ltype.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705080017
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tool to display left justified texts.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ltype )

  \ XXX UNDER DEVELOPMENT
  \ Adapted from Galope <print.fs>.

need last-row need /name

  \ export

variable #ltyped   \ chars displayed in the current line.

variable #indented   \ Indented chars in the current line.

: ltyped+ ( u -- ) #ltyped +! ;

: indented+ ( u -- ) #indented +! ;

: (.word ( ca len -- ) dup ltyped+ type ;

: .char ( c -- ) emit 1 ltyped+ ;

: not-at-home? ( -- 0f ) xy + ;

  \ export

: no-ltyped ( -- ) #ltyped off #indented off ;

: ltype-home ( -- ) home no-ltyped ;

: ltype-page ( -- ) page ltype-home ;

: ltype-start-of-line ( -- )
  #ltyped @ trm+move-cursor-left no-ltyped ;

  \ : ltype-cr ( -- ) not-at-home? if  cr  then  no-ltyped ;
  \ XXX OLD first version

  \ hide

: at-last-start-of-line? ( -- f )
  xy last-row = swap 0= and ;  -->

( ltype )

: not-at-start-of-line? ( -- f ) column 0<> ;

: ltype-cr? ( -- f ) not-at-home? not-at-start-of-line? and ;
  \ XXX FIXME -- 2012-09-30 what this was for?:
  \ at-last-start-of-line? 0= or

  \ export

defer (ltype-cr ' (ltype-cr ' cr defer!

: ltype-cr ltype-cr? if (ltype-cr then no-ltyped ;

variable ltype-width

  \ hide

: previous-word? ( -- f ) #ltyped @ #indented @ > ;

: ?space ( -- ) previous-word? if bl .char then ;

: current-ltype-width ( -- u ) ltype-width @ ?dup ?exit cols ;

: too-long? ( u -- f ) 1+ #ltyped @ + current-ltype-width > ;

: .word ( ca len -- )
  dup too-long? if ltype-cr else ?space then (.word ;

: (ltype-indentation ( u -- )
  dup trm+move-cursor-right dup indented+ ltyped+ ;  -->

( ltype )

  \ export

: ltype-indentation ( u -- ) ?dup 0exit (ltype-indentation ;

  \ hide

: >word ( ca1 len1 ca2 len2 -- ca2 len2 ca1 len4 )
  \ ca1 len1 = Text, from the start of its first word.
  \ ca2 len2 = Same text, from the char after its first word.
  \ ca1 len4 = First word of the text.
  tuck 2>r -  2r> 2swap ;

: first-word ( ca1 len1 -- ca2 len2 ca3 len3 ) /name >word ;

: (ltype ( ca1 len1 -- ca2 len2 ) first-word .word ;

  \ export

: ltype ( ca len --) begin dup while (ltype repeat 2drop ;

  \ Suggested usage in the application:

  \ 4 value indentation
  \ : paragraph ( ca len -- )
  \   ltype-cr indentation ltype-indentation ltype ;

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

  \ vim: filetype=soloforth
