  \ display.print.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705071832
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

( print )

  \ XXX UNDER DEVELOPMENT
  \ Adapted from Galope <print.fs>.

need last-row need /name

  \ export

variable #printed   \ Printed chars in the current line.

variable #indented   \ Indented chars in the current line.

: printed+ ( u -- ) #printed +! ;

: indented+ ( u -- ) #indented +! ;

: (.word ( ca len -- ) dup printed+ type ;

: .char ( c -- ) emit 1 printed+ ;

: not-at-home? ( -- 0f ) xy + ;

  \ export

: no-printed ( -- ) #printed off #indented off ;

: print-home ( -- ) home no-printed ;

: print-page ( -- ) page print-home ;

: print-start-of-line ( -- )
  #printed @ trm+move-cursor-left no-printed ;

  \ : print-cr ( -- ) not-at-home? if  cr  then  no-printed ;
  \ XXX OLD first version

  \ hide

: at-last-start-of-line? ( -- f )
  xy last-row = swap 0= and ;  -->

( print )

: not-at-start-of-line? ( -- f ) column 0<> ;

: print-cr? ( -- f ) not-at-home? not-at-start-of-line? and ;
  \ XXX FIXME -- 2012-09-30 what this was for?:
  \ at-last-start-of-line? 0= or

  \ export

defer (print-cr ' (print-cr ' cr defer!

: print-cr print-cr? if (print-cr then no-printed ;

variable print-width

  \ hide

: previous-word? ( -- f ) #printed @ #indented @ > ;

: ?space ( -- ) previous-word? if bl .char then ;

: current-print-width ( -- u ) print-width @ ?dup ?exit cols ;

: too-long? ( u -- f ) 1+ #printed @ + current-print-width > ;

: .word ( ca len -- )
  dup too-long? if print-cr else ?space then (.word ;

: (print-indentation ( u -- )
  dup trm+move-cursor-right dup indented+ printed+ ;  -->

( print )

  \ export

: print-indentation ( u -- ) ?dup 0exit (print-indentation ;

  \ hide

: >word ( ca1 len1 ca2 len2 -- ca2 len2 ca1 len4 )
  \ ca1 len1 = Text, from the start of its first word.
  \ ca2 len2 = Same text, from the char after its first word.
  \ ca1 len4 = First word of the text.
  tuck 2>r -  2r> 2swap ;

: first-word ( ca1 len1 -- ca2 len2 ca3 len3 ) /name >word ;

: (print ( ca1 len1 -- ca2 len2 ) first-word .word ;

  \ export

: print ( ca len --) begin dup while (print repeat 2drop ;

  \ Suggested usage in the application:

  \ 4 value indentation
  \ : paragraph ( ca len -- )
  \   print-cr indentation print-indentation print ;

  \ ===========================================================
  \ Change log

  \ 2017-02-03: Compact the code, saving two blocks.
  \
  \ 2017-04-20: Review. Modify layout.
  \
  \ 2017-05-07: Improve documentation.

  \ vim: filetype=soloforth
