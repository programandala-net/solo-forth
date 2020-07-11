  \ keyboard.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007112242
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the keyboard.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( at-accept clear-accept set-accept )

  \ XXX UNDER DEVELOPMENT
  \ Common code for several versions of `accept`
  \
  \ 2016-03-13: copied from the kernel, in
  \ order to make it optional in the future.

need 2variable 2variable accept-xy
  \ coordinates of the edited string

  \
  \ accept-xy ( -- a )
  \
  \ A `variable`. _a_ is the address of a double cell containing
  \ the cursor position at the start of the most recent
  \ `accept`.
  \

variable accept-buffer
  \ address of the edited string

  \
  \ accept-buffer ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ buffer address used by the latest execution of `accept`.
  \

variable /accept          \ max length of the edited string

  \
  \ /accept ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ buffer max length used by the latest execution of `accept`.
  \

variable >accept          \ offset to the cursor position

  \
  \ >accept ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ offset of the cursor in the string being edited by
  \ `accept`.
  \

: at-accept ( -- ) accept-xy 2@ at-xy ;

  \
  \ at-accept ( -- )
  \
  \ Set the cursor position at the start of the most recent
  \ `accept`.
  \

variable span

  \
  \ span ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ count of characters actually received and stored by the
  \ last execution of some words.  Originally ``span`` is used
  \ by ``expect``, which is not implemented in Solo Forth.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE EXT,
  \ obsolescent).
  \

: clear-accept ( -- )
  at-accept span @ spaces at-accept span off ;

  \
  \ clear-accept ( -- )
  \
  \ Clear the string currently edited by `accept`.
  \

: set-accept ( ca1 len1 -- ca1' )
  clear-accept /accept @ min ( ca1 len1' )
  dup span ! 2dup fartype
  dup >r
  accept-buffer @ ( ca1 len1' ca2 )
  smove accept-buffer @ ( ca2 )
  r> + ( ca1' ) ;

  \
  \ set-accept ( ca1 len1 -- ca1' )
  \
  \ Set string _ca len_ as the string being edited by `accept`.
  \ Return the address _ca1'_ after its last character.
  \

( acceptx )

  \ XXX UNDER DEVELOPMENT
  \
  \ Alternative version of `accept` with more editing features
  \
  \ 2016-03-13: copied from the kernel, in
  \ order to make it optional in the future.

need at-accept need set-accept need toggle-capslock

: .acceptx ( -- )

  accept-buffer @ >accept @ at-accept type
    \ Display the start of the string, before the cursor.

  1 inverse  >accept @ span @ <
  if accept-buffer @ >accept @ + c@ emit  else  space  then
  0 inverse
    \ Display the cursor.

  accept-buffer @ span @ >accept @ 1+ min /string type ;
    \ Display the end of the string, after the cursor.

: accept-edit ( -- ) clear-accept init-accept ;
: accept-left ( -- ) ;
: accept-right ( -- ) ;
: accept-up ( -- ) ;
: accept-down ( -- ) ;
: accept-delete ( -- ) ;  -->

( acceptx )

create accept-commands ] noop noop noop noop noop noop
toogle-capslock accept-edit accept-left accept-right
accept-down accept-up accept-delete noop noop noop noop noop
noop noop noop noop noop noop noop noop noop noop noop noop [

: >accept-command ( c -- a ) cells accept-commands + ;
: accept-command ( c -- ) >accept-command perform ;

: init-acceptx ( ca len -- )
  /accept ! accept-buffer ! >accept off xy accept-xy 2! ; -->

( acceptx )

: (acceptx ( ca len -- len' ) 2dup init-accept

  over + over ( bot eot cur )
  begin  key dup 13 <> \ not carriage return?
  while
    dup 12 =  \ delete?
    if    drop  >r over r@ < dup  \ any chars?
          if  8 dup emit  bl emit  emit  then  r> +
    else  \ printable
          >r  2dup <>  \ more?
          if r@ over c!  char+  r@ emit
          then rdrop
    then
  repeat  drop nip swap - ;

: acceptx ( ca len -- len' )
  span off ?dup 0= if drop 0 else (acceptx then ;

  \ XXX TMP -- for debugging:

  \ : ax ( -- ) ['] acceptx ['] accept defer! ;
  \ : a0 ( -- ) ['] default-accept ['] accept defer! ;

( nuf? aborted? break? -keys new-key new-key- )

unneeding nuf? ?( need aborted? need 'cr'

: nuf? ( -- f ) 'cr' aborted? ; ?)

  \ Credit:
  \
  \ Code adapted from lpForth and Forth Dimensions (volume 10,
  \ number 1, page 29, 1988-05).

  \ XXX OLD -- Classic definition:
  \
  \ : nuf? ( -- f ) key? dup if  key 2drop key 'cr' = then ;

  \ doc{
  \
  \ nuf? ( -- f ) "nuf-question"
  \
  \ If no key is pressed return `false`.  If a key is pressed,
  \ discard it and wait for a second key. Then return `true` if
  \ it's a carriage return, else return `false`.
  \
  \ Usage example:
  \
  \ ----
  \ : listing ( -- )
  \   begin  ." bla " nuf?  until  ." Aborted" ;
  \ ----
  \
  \ See: `aborted?`.
  \
  \ }doc

unneeding aborted? ?(

: aborted? ( c -- f )
  key? dup if key 2drop key = else nip then ; ?)

  \ doc{
  \
  \ aborted? ( c -- f ) "aborted-question"
  \
  \ If no key is pressed return `false`.  If a key is pressed,
  \ discard it and wait for a second key. Then return `true` if
  \ it's _c_, else return `false`.
  \
  \ ``aborted?`` is a useful factor of `nuf?`.
  \
  \ Usage example:

  \ ----
  \ : listing ( -- )
  \   begin  ." bla "  bl aborted?  until  ." Aborted" ;
  \ ----

  \ }doc

unneeding break? ?(

  \ XXX UNDER DEVELOPMENT
  \ XXX TODO try

: break? ( -- f ) key? dup if key 2drop break-key? then ; ?)

unneeding -keys ?(

code -keys ( -- )
  FD c, CB c, 01 c, 86 08 05 * + c, jpnext, end-code ?)
    \ 01 iy 5 resx, \ res 5,(iy+$01)
    \ Reset bit 5 of system variable FLAGS.

  \ Credit:
  \ Adapted from Galope.

  \ doc{
  \
  \ -keys ( -- ) "minus-keys"
  \
  \ Remove all keys from the keyboard buffer.
  \
  \ See: `key?`, `new-key`, `new-key-`, `key`, `xkey`.
  \
  \ }doc

unneeding new-key need -keys ?\ : new-key ( -- c ) -keys key ;

  \ doc{
  \
  \ new-key ( -- c )
  \
  \ Remove all keys from the keyboard buffer, then return
  \ character _c_ of the key struck, a member of the a member
  \ of the defined character set.
  \
  \ See: `new-key-`, `key`, `xkey`, `-keys`.
  \
  \ }doc

unneeding new-key- ?( need new-key need -keys

: new-key- ( -- ) new-key drop -keys ; ?)

  \ doc{
  \
  \ new-key- ( -- ) "new-key-minus"
  \
  \ Remove all keys from the keyboard buffer, then wait for a
  \ key press and discard it. Finally remove all keys from the
  \ keyboard buffer.
  \
  \ See: `new-key`, `key`, `xkey`, `-keys`.
  \
  \ }doc

( /kk kk-ports kk, kk@ )

  \ Credit:
  \ Adapted from Afera.

  \ ===========================================================
  \ Description

  \ Some tools to manage key presses. An improved and detailed
  \ implementation can be found in the Tron 0xF game
  \ (http://programandala.net/en.program.tron_0xf.html).
  \
  \ "kk" stands for "keyboard key". This notation was chosen
  \ first in order to prevent future name clashes with standard
  \ words which uses the "k-" prefix, and second because these
  \ words manage only physical keys of the keyboard, not key
  \ combinations.
  \
  \ ===========================================================

defined /kk ?\ 4 cconstant /kk

  \ doc{
  \
  \ /kk ( -- n ) "slash-k-k"
  \
  \ _n_ is the number of bytes ocuppied by every key stored in
  \ `kk-ports`: 3 (smaller and slower table) or 4 (bigger and
  \ faster table).
  \
  \ There are two versions of `kk,` and `kk@`. They depend on
  \ the value of `/kk`.
  \
  \ The application can define `/kk` before needing `kk-ports`;
  \ otherwise it will be defined as a `cconstant` with value 4.
  \
  \ }doc

  \ ............................................
  \ Method 1: smaller but slower

  \ Every key identifier occupies 3 bytes in the table (total
  \ size is 120 bytes)

/kk 3 <> ?( : kk, ( b a -- ) , c, ;
            : kk@ ( a1 -- b a2 ) dup c@ swap 1+ @ ;  ?)

  \ XXX TODO -- write this version of `kk@` in Z80

  \ ............................................
  \ Method 2: bigger but faster

  \ Every key identifier occupies 4 bytes in the table (total
  \ size is 160 bytes)

/kk 4 <> ?( need alias ' 2, alias kk, ( b a -- )
                       ' 2@ alias kk@ ( a1 -- b a2 ) ?)

  \ doc{
  \
  \ kk, ( b a -- ) "k-k-comma"
  \
  \ Compile key definition _b a_ (bitmask and port) into table
  \ `kk-ports`. The actual definition of ``kk,`` depends on the
  \ value of `/kk`.
  \
  \ See: `kk@`, `/kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk@ ( a1 -- b a2 ) "k-k-fetch"
  \
  \ Fetch a key definition _b a2_ (bitmask and port) from item
  \ _a1_ of table `kk-ports`.  The actual definition of ``kk@``
  \ depends on the value of `/kk`.
  \
  \ See: `kk,`, `/kk`, `kk-ports`.
  \
  \ }doc

  \ ............................................

$01 $F7FE 2constant kk-1  $02 $F7FE 2constant kk-2
$04 $F7FE 2constant kk-3  $08 $F7FE 2constant kk-4
$10 $F7FE 2constant kk-5

  \ doc{
  \
  \ kk-1 ( -- b a ) "k-k-1"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "1" with `pressed?`.
  \
  \ See: `kk-1#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-2 ( -- b a ) "k-k-2"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "2" with `pressed?`.
  \
  \ See: `kk-2#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-3 ( -- b a ) "k-k-3"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "3" with `pressed?`.
  \
  \ See: `kk-3#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-4 ( -- b a ) "k-k-4"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "4" with `pressed?`.
  \
  \ See: `kk-4#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-5 ( -- b a ) "k-k-5"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "5" with `pressed?`.
  \
  \ See: `kk-5#`, `#kk`, `kk-ports`.
  \
  \ }doc

$01 $FBFE 2constant kk-q  $02 $FBFE 2constant kk-w
$04 $FBFE 2constant kk-e  $08 $FBFE 2constant kk-r
$10 $FBFE 2constant kk-t

  \ doc{
  \
  \ kk-q ( -- b a ) "k-k-Q"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Q" with `pressed?`.
  \
  \ See: `kk-q#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-w ( -- b a ) "k-k-W"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "W" with `pressed?`.
  \
  \ See: `kk-w#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-e ( -- b a ) "k-k-E"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "E" with `pressed?`.
  \
  \ See: `kk-e#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-r ( -- b a ) "k-k-R"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "R" with `pressed?`.
  \
  \ See: `kk-r#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-t ( -- b a ) "k-k-T"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "T" with `pressed?`.
  \
  \ See: `kk-t#`, `#kk`, `kk-ports`.
  \
  \ }doc

$01 $FDFE 2constant kk-a  $02 $FDFE 2constant kk-s
$04 $FDFE 2constant kk-d  $08 $FDFE 2constant kk-f
$10 $FDFE 2constant kk-g  -->

  \ doc{
  \
  \ kk-a ( -- b a ) "k-k-A"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "A" with `pressed?`.
  \
  \ See: `kk-a#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-s ( -- b a ) "k-k-S"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "S" with `pressed?`.
  \
  \ See: `kk-s#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-d ( -- b a ) "k-k-D"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "D" with `pressed?`.
  \
  \ See: `kk-d#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-f ( -- b a ) "k-k-F"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "F" with `pressed?`.
  \
  \ See: `kk-f#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-g ( -- b a ) "k-k-G"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "G" with `pressed?`.
  \
  \ See: `kk-g#`, `#kk`, `kk-ports`.
  \
  \ }doc

( kk-ports )

$01 $FEFE 2constant kk-cs  $02 $FEFE 2constant kk-z
$04 $FEFE 2constant kk-x   $08 $FEFE 2constant kk-c
$10 $FEFE 2constant kk-v

  \ doc{
  \
  \ kk-cs ( -- b a ) "k-k-caps-shift"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Caps Shift" with `pressed?`.
  \
  \ See: `kk-cs#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-z ( -- b a ) "k-k-Z"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Z" with `pressed?`.
  \
  \ See: `kk-z#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-x ( -- b a ) "k-k-X"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "X" with `pressed?`.
  \
  \ See: `kk-x#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-c ( -- b a ) "k-k-C"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "C" with `pressed?`.
  \
  \ See: `kk-c#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-v ( -- b a ) "k-k-V"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "V" with `pressed?`.
  \
  \ See: `kk-v#`, `#kk`, `kk-ports`.
  \
  \ }doc

$01 $EFFE 2constant kk-0  $02 $EFFE 2constant kk-9
$04 $EFFE 2constant kk-8  $08 $EFFE 2constant kk-7
$10 $EFFE 2constant kk-6

  \ doc{
  \
  \ kk-0 ( -- b a ) "k-k-0"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "0" with `pressed?`.
  \
  \ See: `kk-0#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-9 ( -- b a ) "k-k-9"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "9" with `pressed?`.
  \
  \ See: `kk-9#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-8 ( -- b a ) "k-k-8"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "8" with `pressed?`.
  \
  \ See: `kk-8#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-7 ( -- b a ) "k-k-7"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "7" with `pressed?`.
  \
  \ See: `kk-7#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-6 ( -- b a ) "k-k-6"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "6" with `pressed?`.
  \
  \ See: `kk-6#`, `#kk`, `kk-ports`.
  \
  \ }doc

$01 $DFFE 2constant kk-p  $02 $DFFE 2constant kk-o
$04 $DFFE 2constant kk-i  $08 $DFFE 2constant kk-u
$10 $DFFE 2constant kk-y

  \ doc{
  \
  \ kk-p ( -- b a ) "k-k-P"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "P" with `pressed?`.
  \
  \ See: `kk-p#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-o ( -- b a ) "k-k-O"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "O" with `pressed?`.
  \
  \ See: `kk-o#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-i ( -- b a ) "k-k-I"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "I" with `pressed?`.
  \
  \ See: `kk-i#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-u ( -- b a ) "k-k-U"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "U" with `pressed?`.
  \
  \ See: `kk-u#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-y ( -- b a ) "k-k-Y"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Y" with `pressed?`.
  \
  \ See: `kk-y#`, `#kk`, `kk-ports`.
  \
  \ }doc

$01 $BFFE 2constant kk-en  $02 $BFFE 2constant kk-l
$04 $BFFE 2constant kk-k   $08 $BFFE 2constant kk-j
$10 $BFFE 2constant kk-h

  \ doc{
  \
  \ kk-en ( -- b a ) "k-k-enter"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Enter" with `pressed?`.
  \
  \ See: `kk-en#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-l ( -- b a ) "k-k-L"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "L" with `pressed?`.
  \
  \ See: `kk-l#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-k ( -- b a ) "k-k-K"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "K" with `pressed?`.
  \
  \ See: `kk-k#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-j ( -- b a ) "k-k-J"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "J" with `pressed?`.
  \
  \ See: `kk-j#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-h ( -- b a ) "k-k-H"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "H" with `pressed?`.
  \
  \ See: `kk-h#`, `#kk`, `kk-ports`.
  \
  \ }doc

$01 $7FFE 2constant kk-sp $02 $7FFE 2constant kk-ss
$04 $7FFE 2constant kk-m  $08 $7FFE 2constant kk-n
$10 $7FFE 2constant kk-b  -->

  \ doc{
  \
  \ kk-sp ( -- b a ) "k-k-space"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Space" with `pressed?`.
  \
  \ See: `kk-sp#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-ss ( -- b a ) "k-k-symbol-shift"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "Symbol Shift" with `pressed?`.
  \
  \ See: `kk-ss#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-m ( -- b a ) "k-k-M"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "M" with `pressed?`.
  \
  \ See: `kk-m#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-n ( -- b a ) "k-k-N"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "N" with `pressed?`.
  \
  \ See: `kk-n#`, `#kk`, `kk-ports`.
  \
  \ }doc

  \ doc{
  \
  \ kk-b ( -- b a ) "k-k-B"
  \
  \ Return key bitmask _b_ and keyboard row port _a_ needed for
  \ reading the physical key "B" with `pressed?`.
  \
  \ See: `kk-b#`, `#kk`, `kk-ports`.
  \
  \ }doc

( kk-ports )

40 cconstant #kk

  \ doc{
  \
  \ #kk ( -- n ) "dash-k-k"
  \
  \ A `cconstant`. _n_ is the number of keyboard keys, i.e. the
  \ number of physical rubber keys on the keyboard of the
  \ original ZX Spectrum: 40.
  \
  \ See: `kk-ports`, `kk-chars`, `kk-0#`, `kk-0`, `kk-1#`,
  \ `kk-1`, `kk-2#`, `kk-2`, `kk-3#`, `kk-3`, `kk-4#`, `kk-4`,
  \ `kk-5#`, `kk-5`, `kk-6#`, `kk-6`, `kk-7#`, `kk-7`, `kk-8#`,
  \ `kk-8`, `kk-9#`, `kk-9`, `kk-a#`, `kk-a`, `kk-b#`, `kk-b`,
  \ `kk-c#`, `kk-c`, `kk-cs#`, `kk-cs`, `kk-d#`, `kk-d`,
  \ `kk-e#`, `kk-e`, `kk-en#`, `kk-en`, `kk-f#`, `kk-f`,
  \ `kk-g#`, `kk-g`, `kk-h#`, `kk-h`, `kk-i#`, `kk-i`, `kk-j#`,
  \ `kk-j`, `kk-k#`, `kk-k`, `kk-l#`, `kk-l`, `kk-m#`, `kk-m`,
  \ `kk-n#`, `kk-n`, `kk-o#`, `kk-o`, `kk-p#`, `kk-p`, `kk-q#`,
  \ `kk-q`, `kk-r#`, `kk-r`, `kk-s#`, `kk-s`, `kk-sp#`,
  \ `kk-sp`, `kk-ss#`, `kk-ss`, `kk-t#`, `kk-t`, `kk-u#`,
  \ `kk-u`, `kk-v#`, `kk-v`, `kk-w#`, `kk-w`, `kk-x#`, `kk-x`,
  \ `kk-y#`, `kk-y`, `kk-z`.
  \
  \ }doc

create kk-ports

kk-1  kk,  kk-2  kk,  kk-3 kk,  kk-4 kk,  kk-5 kk,
kk-q  kk,  kk-w  kk,  kk-e kk,  kk-r kk,  kk-t kk,
kk-a  kk,  kk-s  kk,  kk-d kk,  kk-f kk,  kk-g kk,
kk-cs kk,  kk-z  kk,  kk-x kk,  kk-c kk,  kk-v kk,
kk-0  kk,  kk-9  kk,  kk-8 kk,  kk-7 kk,  kk-6 kk,
kk-p  kk,  kk-o  kk,  kk-i kk,  kk-u kk,  kk-y kk,
kk-en kk,  kk-l  kk,  kk-k kk,  kk-j kk,  kk-h kk,
kk-sp kk,  kk-ss kk,  kk-m kk,  kk-n kk,  kk-b kk,

  \ doc{
  \
  \ kk-ports ( -- a ) "k-k-ports"
  \
  \ A table that contains the key definitions (bitmak and port)
  \ of all keys.
  \
  \ The table contains 40 items, one per physical key, and it's
  \ organized by keyboard rows.
  \
  \ Every item occupies 3 or 4 bytes, depending on the value of
  \ `/kk`. The default is 4.
  \
  \ See: `kk,`, `kk@`, `#kk`, `kk-chars`.
  \
  \ }doc

( kk-1# kk-2# kk-3# kk-4# kk-5# kk-q# kk-w# kk-e# kk-r# kk-t# )

need cenum

0 cenum kk-1#  cenum kk-2#  cenum kk-3# cenum kk-4# cenum kk-5#
  cenum kk-q#  cenum kk-w#  cenum kk-e# cenum kk-r# cenum kk-t#
  cenum kk-a#  cenum kk-s#  cenum kk-d# cenum kk-f# cenum kk-g#
  cenum kk-cs# cenum kk-z#  cenum kk-x# cenum kk-c# cenum kk-v#
  cenum kk-0#  cenum kk-9#  cenum kk-8# cenum kk-7# cenum kk-6#
  cenum kk-p#  cenum kk-o#  cenum kk-i# cenum kk-u# cenum kk-y#
  cenum kk-en# cenum kk-l#  cenum kk-k# cenum kk-j# cenum kk-h#
  cenum kk-sp# cenum kk-ss# cenum kk-m# cenum kk-n# cenum kk-b#

drop

( kk-a# kk-s# kk-d# kk-f# kk-g# kk-cs# kk-z# kk-x# kk-c# )

need kk-1#

( kk-v# kk-0# kk-9# kk-8# kk-7# kk-6# kk-p# kk-o# kk-i# kk-u# )

need kk-1#

( kk-y# kk-en# kk-l# kk-k# kk-j# kk-h# kk-sp# kk-ss# kk-m# )

need kk-1#

( kk-n# kk-b# )

need kk-1#

  \ doc{
  \
  \ kk-1# ( -- n ) "k-k-1-dash"
  \
  \ Return index _n_ of the physical key "1" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-1`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-2# ( -- n ) "k-k-2-dash"
  \
  \ Return index _n_ of the physical key "2" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-2`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-3# ( -- n ) "k-k-3-dash"
  \
  \ Return index _n_ of the physical key "3" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-3`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-4# ( -- n ) "k-k-4-dash"
  \
  \ Return index _n_ of the physical key "4" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-4`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-5# ( -- n ) "k-k-5-dash"
  \
  \ Return index _n_ of the physical key "5" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-5`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-q# ( -- n ) "k-k-Q-dash"
  \
  \ Return index _n_ of the physical key "Q" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-q`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-w# ( -- n ) "k-k-W-dash"
  \
  \ Return index _n_ of the physical key "W" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-w`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-e# ( -- n ) "k-k-E-dash"
  \
  \ Return index _n_ of the physical key "E" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-e`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-r# ( -- n ) "k-k-R-dash"
  \
  \ Return index _n_ of the physical key "R" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-r`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-t# ( -- n ) "k-k-T-dash"
  \
  \ Return index _n_ of the physical key "T" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-t`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-a# ( -- n ) "k-k-A-dash"
  \
  \ Return index _n_ of the physical key "A" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-a`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-s# ( -- n ) "k-k-S-dash"
  \
  \ Return index _n_ of the physical key "S" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-s`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-d# ( -- n ) "k-k-D-dash"
  \
  \ Return index _n_ of the physical key "D" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-d`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-f# ( -- n ) "k-k-F-dash"
  \
  \ Return index _n_ of the physical key "F" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-f`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-g# ( -- n ) "k-k-G-dash"
  \
  \ Return index _n_ of the physical key "G" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-g`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-cs# ( -- n ) "k-k-caps-shift-dash"
  \
  \ Return index _n_ of the physical key "Caps Shift" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-cs`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-z# ( -- n ) "k-k-Z-dash"
  \
  \ Return index _n_ of the physical key "Z" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-z`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-x# ( -- n ) "k-k-X-dash"
  \
  \ Return index _n_ of the physical key "X" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-x`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-c# ( -- n ) "k-k-C-dash"
  \
  \ Return index _n_ of the physical key "C" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-c`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-v# ( -- n ) "k-k-V-dash"
  \
  \ Return index _n_ of the physical key "V" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-v`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-0# ( -- n ) "k-k-0-dash"
  \
  \ Return index _n_ of the physical key "0" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-0`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-9# ( -- n ) "k-k-9-dash"
  \
  \ Return index _n_ of the physical key "9" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-9`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-8# ( -- n ) "k-k-8-dash"
  \
  \ Return index _n_ of the physical key "8" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-8`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-7# ( -- n ) "k-k-7-dash"
  \
  \ Return index _n_ of the physical key "7" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-7`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-6# ( -- n ) "k-k-6-dash"
  \
  \ Return index _n_ of the physical key "6" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-6`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-p# ( -- n ) "k-k-P-dash"
  \
  \ Return index _n_ of the physical key "P" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-p`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-o# ( -- n ) "k-k-O-dash"
  \
  \ Return index _n_ of the physical key "O" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-o`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-i# ( -- n ) "k-k-I-dash"
  \
  \ Return index _n_ of the physical key "I" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-i`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-u# ( -- n ) "k-k-U-dash"
  \
  \ Return index _n_ of the physical key "U" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-u`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-y# ( -- n ) "k-k-Y-dash"
  \
  \ Return index _n_ of the physical key "Y" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-y`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-en# ( -- n ) "k-k-enter-dash"
  \
  \ Return index _n_ of the physical key "Enter" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-en`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-l# ( -- n ) "k-k-L-dash"
  \
  \ Return index _n_ of the physical key "L" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-l`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-k# ( -- n ) "k-k-K-dash"
  \
  \ Return index _n_ of the physical key "K" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-k`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-j# ( -- n ) "k-k-J-dash"
  \
  \ Return index _n_ of the physical key "J" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-j`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-h# ( -- n ) "k-k-H-dash"
  \
  \ Return index _n_ of the physical key "H" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-h`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-sp# ( -- n ) "k-k-space-dash"
  \
  \ Return index _n_ of the physical key "Space" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-sp`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-ss# ( -- n ) "k-k-symbol-shift-dash"
  \
  \ Return index _n_ of the physical key "Symbol Shift" in
  \ tables `kk-chars` and `kk-ports`.
  \
  \ See: `kk-ss`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-m# ( -- n ) "k-k-M-dash"
  \
  \ Return index _n_ of the physical key "M" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-m`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-n# ( -- n ) "k-k-N-dash"
  \
  \ Return index _n_ of the physical key "N" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-n`, `#kk`, `kk#>kk`.
  \
  \ }doc

  \ doc{
  \
  \ kk-b# ( -- n ) "k-k-B-dash"
  \
  \ Return index _n_ of the physical key "B" in tables
  \ `kk-chars` and `kk-ports`.
  \
  \ See: `kk-b`, `#kk`, `kk#>kk`.
  \
  \ }doc

( kk-chars )

create kk-chars '1' c,  '2' c,  '3' c,  '4' c,  '5' c,
                'q' c,  'w' c,  'e' c,  'r' c,  't' c,
                'a' c,  's' c,  'd' c,  'f' c,  'g' c,
                128 c,  'z' c,  'x' c,  'c' c,  'v' c,
                '0' c,  '9' c,  '8' c,  '7' c,  '6' c,
                'p' c,  'o' c,  'i' c,  'u' c,  'y' c,
                129 c,  'l' c,  'k' c,  'j' c,  'h' c,
                130 c,  131 c,  'm' c,  'n' c,  'b' c,

  \ doc{
  \
  \ kk-chars ( -- ca ) "k-k-chars"
  \
  \ _ca_ is the address of a 40-byte table that contains the
  \ characters used as names of the physical keys (one
  \ character per key) and it's organized by keyboard rows, as
  \ follows:

  \ .Keyboard matrix pointed by ``kk-chars``.
  \ |===
  \ | 1          | 2            | 3 | 4 | 5 |
  \ | q          | w            | e | r | t |
  \ | a          | s            | d | f | g |
  \ | Caps Shift | z            | x | c | v |
  \ | 0          | 9            | 8 | 7 | 6 |
  \ | p          | o            | i | u | y |
  \ | Enter      | l            | k | j | h |
  \ | Space      | Symbol Shift | m | n | b |
  \ |===

  \ The first 4 UDG codes displayed after the default
  \ configuration of `last-font-char` are used for the keys
  \ whose names are not a printable character, as follows:

  \ .Items of ``kk-chars`` used as names of special keys.
  \ |===
  \ | Byte offset | UDG code | Key
  \
  \ | 15          | 128      | Caps Shift
  \ | 30          | 129      | Enter
  \ | 35          | 130      | Space
  \ | 36          | 131      | Symbol Shift
  \ |===

  \ The application should define those UDG with proper icons
  \ to represent the corresponding keys.
  \
  \ See: `#kk`, `kk-ports`.
  \
  \ }doc

( kk#>kk pressed pressed? )

unneeding kk#>kk ?( need kk-ports

: kk#>kk ( n -- b a ) /kk * kk-ports + kk@ ; ?)

  \ doc{
  \
  \ kk#>kk ( n -- b a ) "k-k-dash-to-k-k"
  \
  \ Convert keyboard key number _n_ to its data: key bitmask
  \ _b_ and keyboard row port _a_.
  \
  \ See: `kk-ports`, `/kk`, `kk@`.
  \
  \ }doc

unneeding pressed? ?( need @p

: pressed? ( b a -- f ) @p and 0= ; ?)

  \ doc{
  \
  \ pressed? ( b a -- f ) "pressed-question"
  \
  \ Is a keyboard key _b a_ pressed?  _b_ is the key bitmask
  \ and _a_ is the keyboard row port.
  \
  \ See: `pressed`, `only-one-pressed`.
  \
  \ }doc

unneeding pressed ?( need pressed? need kk-ports need +loop

: pressed ( -- false | b a true )
  false \ by default
  [ kk-ports #kk /kk * bounds swap ] literal literal ?do
    i kk@ pressed? if drop i kk@ 1 leave then /kk
  +loop ; ?)

  \ doc{
  \
  \ pressed ( -- false | b a true )
  \
  \ Return the key identifier _b a_ (key bitmask and keyboard
  \ row port) of the first key from table `kk-ports`
  \ that happens to be pressed, and `true`; if no key is
  \ pressed, return `false`.
  \
  \ See: `only-one-pressed`, `pressed?`.
  \
  \ }doc

( only-one-pressed )

  \ XXX UNDER DEVELOPMENT

  \ The application must define the `/k` constant.

need kk-ports need +loop need 2variable

0. 2variable kk-pressed

: only-one-pressed ( -- false | b a true )

  \ XXX TODO finish

  0. kk-pressed 2! \ none by default
  [ kk-ports #kk /kk * bounds swap ] literal literal
  ?do  i kk@ pressed?  if  kk-pressed 2@ + if
                       then
  /kk +loop
  kk-pressed 2@ 2dup + if 1 else 2drop 0 then ;

  \ doc{
  \
  \ only-one-pressed ( -- false | b a true )
  \
  \ Return the key identifier _b a_ (key bitmask and keyboard
  \ row port) of the only key from table `kk-ports` that
  \ happens to be pressed, and `true`; if no key is pressed or
  \ more than one key is pressed at the same time, return
  \ `false`.
  \
  \ See: `pressed`, `pressed?`.
  \
  \ }doc

( key-edit key-left key-right key-down key-up key-delete )

unneeding key-edit ?\ 7 cconstant key-edit

  \ doc{
  \
  \ key-edit ( -- c )
  \
  \ _c_ is the edit control character, which is obtained by
  \ pressing "Caps Shift + 1" and can be read by `key`.
  \
  \ See: `key-left`, `key-right`, `key-down`, `key-up`,
  \ `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-left ?\ 8 cconstant key-left

  \ doc{
  \
  \ key-left ( -- c )
  \
  \ _c_ is the cursor-left control character, which is obtained
  \ by pressing "Caps Shift + 5" and can be read by `key`.
  \
  \ See: `key-edit`, `key-right`, `key-down`, `key-up`,
  \ `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-right ?\ 9 cconstant key-right

  \ doc{
  \
  \ key-right ( -- c )
  \
  \ _c_ is the cursor-right control character, which is
  \ obtained by pressing "Caps Shift + 8" and can be read by
  \ `key`.
  \
  \ See: `key-edit`, `key-left`, `key-down`, `key-up`,
  \ `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-down ?\ 10 cconstant key-down

  \ doc{
  \
  \ key-down ( -- c )
  \
  \ _c_ is the cursor-down control character, which is obtained
  \ by pressing "Caps Shift + 6" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-up`,
  \ `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-up ?\ 11 cconstant key-up

  \ doc{
  \
  \ key-up ( -- c )
  \
  \ _c_ is the cursor-up control character, which is obtained
  \ by pressing "Caps Shift + 7" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-delete ?\ 12 cconstant key-delete

  \ doc{
  \
  \ key-delete ( -- c )
  \
  \ _c_ is the delete control character, which is obtained by
  \ pressing "Caps Shift + 0" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-up`, `key-enter`, `key-graphics`, `key-true-video`,
  \ `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

( key-enter key-graphics key-true-video key-inverse-video )

unneeding key-enter ?\ 13 cconstant key-enter

  \ doc{
  \
  \ key-enter ( -- c )
  \
  \ _c_ is the enter control character, which is obtained by
  \ pressing "Enter" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-up`, `key-delete`, `key-graphics`, `key-true-video`,
  \ `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-graphics ?\ 15 cconstant key-graphics

  \ doc{
  \
  \ key-graphics ( -- c )
  \
  \ _c_ is the graphics control character, which is obtained by
  \ pressing "Caps Shift + 9" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-up`, `key-delete`, `key-enter`, `key-true-video`,
  \ `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-true-video ?\ 4 cconstant key-true-video

  \ doc{
  \
  \ key-true-video ( -- c )
  \
  \ _c_ is the true-video control character, which is obtained
  \ by pressing "Caps Shift + 3" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-up`, `key-delete`, `key-enter`, `key-graphics`,
  \ `key-inverse-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-inverse-video ?\ 5 cconstant key-inverse-video

  \ doc{
  \
  \ key-inverse-video ( -- c )
  \
  \ _c_ is the inverse-video control character, which is
  \ obtained by pressing "Caps Shift + 4" and can be read by
  \ `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-up`, `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-caps-lock`.
  \
  \ }doc

unneeding key-caps-lock ?\ 6 cconstant key-caps-lock

  \ doc{
  \
  \ key-caps-lock ( -- c )
  \
  \ _c_ is the caps-lock control character, which is obtained
  \ by pressing "Caps Shift + 2" and can be read by `key`.
  \
  \ See: `key-edit`, `key-left`, `key-right`, `key-down`,
  \ `key-up`, `key-delete`, `key-enter`, `key-graphics`,
  \ `key-true-video`, `key-inverse-video`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2016-12-04: Use `cenum` instead of `enum` for `kk-1#`
  \ constant and family. This change saves 40 bytes of data
  \ space and makes the access faster. Add `#>kk`. Document
  \ many words. Improve access by `need`. Define `/kk` by
  \ default. Compact the code, saving two blocks.
  \
  \ 2016-12-24: Add `key-edit`, `key-left`, `key-right`,
  \ `key-down`, `key-up`, `key-delete`, `key-enter`. Rename the
  \ module from <keyboard.fsb> to <keyboard.MISC.fsb>.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-05: Convert `set-accept` to far memory.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-01: Move `span` from the kernel.
  \
  \ 2017-02-15: Update notation in the documentation of `span`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-04: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-11-28: Add `-keys`, `new-key`, `new-key-`.
  \
  \ 2017-12-09: Need `[defined]`, which was moved to the
  \ library.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-02-13: Improve documentation.
  \
  \ 2018-02-14: Fix port of key row Caps Shift-V.  Fix
  \ `key-delete`. Add `key-graphics`, `key-true-video`,
  \ `key-inverse-video`, key-caps-lock`.  Rename `#>kk`
  \ `kk#>kk`. Rename `keys` `#kk`.  Improve documentation.
  \
  \ 2018-02-17: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-06-04: Update: remove trailing closing paren from word
  \ names.  Link `variable` in documentation. Replace
  \ `[defined]` with `defined`, which is the kernel.
  \
  \ 2020-05-18: Update: `+loop` was moved to the library.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library.
  \
  \ 2020-05-25: Replace `r> drop` with `rdrop`.
  \
  \ 2020-06-08: Improve documentation: make _true_ and
  \ _false_ cross-references.
  \
  \ 2020-06-15: Improve documentation.
  \
  \ 2020-07-11: Add title to tables. Improve documentation.

  \ vim: filetype=soloforth
