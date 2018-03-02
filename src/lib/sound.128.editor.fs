  \ sound.128.editor.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803022237
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A simple 128k sound editor.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( edit-sound )

get-current  forth-wordlist set-current

need :noname need c1+! need c1-! need inverse need case
need play need /sound need column need silence need cvariable
need key-left need key-right need key-down need key-up need c!>
need c@+ need key-enter need binary

wordlist constant edit-sound-wordlist
edit-sound-wordlist dup >order set-current

variable sound  cvariable register#
  \ `sound`: address of the sound being edited
  \ `register#`: register (0..13) being edited

key-enter cconstant toggle-key
      'Q' cconstant quit-key
      'k' cconstant keyset-key
  \ Keys common to all keysets.

0 cconstant up-key    0 cconstant down-key
0 cconstant left-key  0 cconstant right-key -->
  \ Keys changed by the keysets.

( edit-sound )

cvariable keyset 4 cconstant /keyset 7 cconstant #keysets

create keysets
  key-up c, key-down c, key-left c, key-right c,
    \ Actual cursor.
  '.' c, 'a' c, 'r' c, 'l' c,
    \ English Dvorak.
  '.' c, 'a' c, 'h' c, 'l' c,
    \ Spanish Dvorak.
  'q' c, 'a' c, 'o' c, 'p' c,
    \ QWERTY.
  '7' c, '6' c, '5' c, '8' c,
    \ Cursor digits.
  '3' c, '4' c, '1' c, '2' c,
    \ Sinclair 1.
  '8' c, '9' c, '5' c, '6' c,
    \ Sinclair 2.

: set-keyset ( n -- ) dup keyset c! /keyset * keysets +
                      c@+ c!> up-key   c@+ c!> down-key
                      c@+ c!> left-key c@+ c!> right-key drop ;

0 set-keyset

: next-keyset ( -- )
  keyset c@ 1+ dup #keysets < and set-keyset ; -->

( edit-sound )

: .legend ( ca len -- ) 11 column - spaces ." = " type cr ;

: .key ( c -- ) case key-left  of ." Left"  endof
                     key-right of ." Right" endof
                     key-up    of ." Up"    endof
                     key-down  of ." Down"  endof
                     key-enter of ." Enter" endof
                     \ bl        of ." Space" endof \ XXX OLD
                     dup emit endcase ;

: .help ( -- ) 0 18 at-xy
  up-key   .key space down-key  .key s" navigate"     .legend
  left-key .key space right-key .key s" alter"        .legend
  toggle-key                    .key s" play/silence" .legend
  quit-key                      .key s" quit"         .legend
  keyset-key                    .key s" keyset"       .legend
  ." # $ %"                          s" radix"        .legend ;

-->

( edit-sound )

:noname ( -- ) ." Vol. env. shape" ;

  \ 0Dh = Volume Envelope shape (0-15)

  \ Writing to this register (re-)starts the envelope.
  \ Additionally, the written value specifies the envelope
  \ shape, the four bits have the following meaning:

  \   CONT ATT ALT HLD
  \     0   0   X   X  \_________  0-3 (same as 9)
  \     0   1   X   X  /_________  4-7 (same as F)
  \     1   0   0   0  \\\\\\\\\\  8   (Repeating)
  \     1   0   0   1  \_________  9
  \     1   0   1   0  \/\/\/\/\/  A   (Repeating)
  \     1   0   1   1  \"""""""""  B
  \     1   1   0   0  //////////  C   (Repeating)
  \     1   1   0   1  /"""""""""  D
  \     1   1   1   0  /\/\/\/\/\  E   (Repeating)
  \     1   1   1   1  /_________  F

:noname ( -- ) ." Vol. env. freq., hi" ;
:noname ( -- ) ." Vol. env. freq., lo" ;
:noname ( -- ) ." C vol. 1-15 (16=env.)" ;
:noname ( -- ) ." B vol. 1-15 (16=env.)" ;
:noname ( -- ) ." A vol. 1-15 (16=env.)" ;
:noname ( -- ) ." Mixer control" ;

  \ |===
  \ |  Bit | Explanation            | Usage

  \ |  0   | Channel A tone enable  | 0=Enable, 1=Disable
  \ |  1   | Channel B tone enable  | 0=Enable, 1=Disable
  \ |  2   | Channel C tone enable  | 0=Enable, 1=Disable
  \ |  3   | Channel A noise enable | 0=Enable, 1=Disable
  \ |  4   | Channel B noise enable | 0=Enable, 1=Disable
  \ |  5   | Channel C noise enable | 0=Enable, 1=Disable
  \ |  6   | I/O port A mode        | 0=Input, 1=Output
  \ |  7   | I/O port B mode        | 0=Input, 1=Output
  \ |===

                \ <------------------------------>
:noname ( -- ) ." Noise freq. 0-31" ;
:noname ( -- ) ." C tone freq., hi 0-15" ;
:noname ( -- ) ." C tone freq., lo 0-255" ;
:noname ( -- ) ." B tone freq., hi 0-15" ;
:noname ( -- ) ." B tone freq., lo 0-255" ;
:noname ( -- ) ." A tone freq., hi 0-15" ;
:noname ( -- ) ." A tone freq., lo 0-255" ;

create label  , , , , , , , , , , , , , ,  -->
  \ Execution tokens of the register labels.

( edit-sound )

: .label ( n -- ) cells label + perform ;
  \ Display the label of register _n_.

: .register ( n -- )
  >r 0 r@ at-xy sound @ r@ + c@ 8 .r space r> .label cr ;
  \ Display register _n_.

: (.menu-register ( n f -- ) inverse .register 0 inverse ;
  \ Display register _n_ with inverse video _f_.

: .current-register ( -- ) register# c@ true (.menu-register ;
  \ Display the current register.

: .menu-register ( n -- ) dup register# c@ = (.menu-register ;
  \ Display register _n_.

: .sound ( -- ) /sound 0 ?do i .menu-register loop ;
  \ Display the data of the currently edited sound.

: register@ ( -- n ) register# c@ dup .register ;
  \ Display the currently edited register on its position,
  \ without inverse video, and return its value.

: register! ( n -- ) dup register# c! .menu-register ;
  \ Make _n_ the currently edited register.

: next-register ( -- )
  register@ 1+ dup /sound <> and register! ;
  \ Increase the number of the currently edited register.
  \ Set it to zero if it was the maximum number.

: previous-register ( -- )
  register@ 1- dup 0< if drop /sound 1- then register! ;
  \ Decrease the number of the currently edited register.
  \ Set it to the maximum number if it was zero.

: register ( -- ca ) sound @ register# c@ + ;
  \ Address of the currently edited register.

-->

( edit-sound )

: increase-value ( -- ) register c1+! .current-register ;
  \ Increase the value of the currently edited register.

: decrease-value ( -- ) register c1-! .current-register ;
  \ Decrease the value of the currently edited register.

variable sounding  sounding off
  \ Is the sound sounding?

: toggle ( -- ) sounding @ dup if   silence else sound @ play
                               then 0= sounding ! ;
  \ Toggle the sound.


-->

( edit-sound )

forth-wordlist set-current

: edit-sound ( ca -- )
  sound ! 0 register# c! page .sound cr .help
  begin key
    case quit-key   of decimal quit      endof
         toggle-key of toggle            endof
         left-key   of decrease-value    endof
         right-key  of increase-value    endof
         down-key   of next-register     endof
         up-key     of previous-register endof
         keyset-key of next-keyset .help endof
         '#'        of decimal .sound    endof
         '$'        of hex     .sound    endof
         '%'        of binary  .sound    endof endcase again ;

  \ doc{
  \
  \ edit-sound ( ca -- )
  \
  \ Start a simple editor to edit the 14-byte 128K-sound
  \ definition stored at _ca_. Instructions are displayed.
  \
  \ Usage example:
  \
  \ ----
  \ need train-sound need >body
  \ ' train-sound >body edit-sound
  \ ----
  \
  \ See: `sound`, `play`.
  \
  \ }doc

set-current previous

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
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-12-09: Need `/sound` instead of redefining it, since
  \ `[defined]` is moved to the library.
  \
  \ 2018-02-21: Update source layout (remove double spaces).
  \ Replace `value` with `cconstant`. Add `silence` option.
  \ Improve menu.
  \
  \ 2018-02-28: Document.
  \
  \ 2018-03-01: Fix `.legend`. Implement key sets.
  \
  \ 2018-03-02: Improve key sets. Improve updating the
  \ registers on the screen. Add keys to change the radix.

  \ vim: filetype=soloforth
