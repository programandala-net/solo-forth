  \ sound.128.editor.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221412
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A simple 128k sound editor.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( edit-sound )

get-current  forth-wordlist set-current

need :noname need c1+! need c1-! need inverse need case
need value need play  [defined] /sound ?\ 14 constant /sound

wordlist constant edit-sound-wordlist
edit-sound-wordlist dup >order set-current

variable sound  variable register
  \ `sound`: address of the sound being edited
  \ `register`: register (0..13) being edited

'q' value quit-key  'p' value play-key
  8 value left-key    9 value right-key
 10 value down-key   11 value up-key

  \ : key>name ( c -- ca len )
  \ XXX TODO --

: .help ( -- )
  quit-key emit ."           - quit" cr
  play-key emit ."           - play" cr
  ." left/right - decrease/increase value" cr
  ." up/down    - previous/next register" cr ;
  \ XXX TODO -- print name of control keys

-->

( edit-sound )

:noname ( -- ) ." Env." ;     \ XXX TODO --
:noname ( -- ) ." Env. T." ;  \ XXX TODO --
:noname ( -- ) ." Env. P." ;  \ XXX TODO --
:noname ( -- ) ." C volume" ;
:noname ( -- ) ." B volume" ;
:noname ( -- ) ." A volume" ;
:noname ( -- ) ." Mixer" ;
:noname ( -- ) ." Noise volume" ;
:noname ( -- ) ." C tone" ;
:noname ( -- ) ." C fine tone" ;
:noname ( -- ) ." B tone" ;
:noname ( -- ) ." B fine tone" ;
:noname ( -- ) ." A tone" ;
:noname ( -- ) ." A fine tone" ;

create label  , , , , , , , , , , , , , ,  -->
  \ Execution tokens of the register labels.

( edit-sound )

: .label ( n -- ) cells label + perform ;
  \ Print the label of register _n_.

: .register ( n -- )
  >r 0 r@ at-xy  sound @ r@ + c@ 4 .r  space r> .label cr ;

: .menu-register ( n -- )
  dup register @ = inverse  .register  0 inverse ;
  \ Print register _n_ of the currently edited sound.

: .sound ( -- ) /sound 0 ?do  i .menu-register  loop ;
  \ Print the data of the currently edited sound.

: register@ ( -- n ) register @ dup .register ;
  \ Print the currently edited register on its position,
  \ without inverse video, and return its value.

: register! ( n -- ) dup register ! .menu-register ;
  \ Make _n_ the currently edited register and update its label
  \ on the screen.

: next-register ( -- )
  register@ 1+ dup /sound <> and register! ;
  \ Increase the number of the currently edited register.
  \ Set it to zero if it was the maximum number.

: previous-register ( -- )
  register@ 1- dup 0< if  drop /sound 1-  then  register! ;
  \ Decrease the number of the currently edited register.
  \ Set it to the maximum number if it was zero.

: >register ( -- ca ) sound @ register @ + ;
  \ Address of the currently edited register.

: increase-value ( -- ) >register c1+! ;
  \ Increase the value of the currently edited register.

: decrease-value ( -- ) >register c1-! ;  -->
  \ Decrease the value of the currently edited register.

( edit-sound )

forth-wordlist set-current

: edit-sound ( a -- )
  sound ! register off  page .sound cr .help
  begin  .sound
    key lower case
      quit-key   of  exit               endof
      play-key   of  sound @ play       endof
      left-key   of  decrease-value     endof
      right-key  of  increase-value     endof
      down-key   of  next-register      endof
      up-key     of  previous-register  endof
    endcase
  again ;

set-current  previous

  \ ===========================================================
  \ Change log

  \ 2016-08-02: Start.
  \
  \ 2017-01-23: Move `edit-sound-test` to the tests module.
  \
  \ 2017-02-01: Use `lower` in `edit-sound`, because `upper`
  \ has been moved to the library.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.

  \ vim: filetype=soloforth
