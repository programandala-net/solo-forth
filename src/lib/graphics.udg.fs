  \ graphics.udg.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703191958
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to User Defined Graphics.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /udg /udg* udg-width default-udg-chars udg> udg! udg: )

[unneeded] /udg ?\ 8 cconstant /udg

  \ doc{
  \
  \ /udg ( -- b )
  \
  \ _b_ is the size of a UDG (User Defined Graphic), in bytes.
  \
  \ See also: `udg-width`, `udg!`.
  \
  \ }doc

[unneeded] /udg* ?\ need 8* need alias ' 8* alias /udg*

  \ doc{
  \
  \ /udg* ( n1 -- n2 )
  \
  \ Multiply _n1_ by the `/udg`, resulting _n2_. Used by
  \ `udg>`.
  \
  \ ``/udg*`` is an `alias` of `8*`.
  \
  \ }doc

[unneeded] udg-width ?\ 8 cconstant udg-width

  \ doc{
  \
  \ udg-width ( -- b )
  \
  \ _b_ is the width of a UDG (User Defined Graphic), in
  \ pixels.
  \
  \ See also: `/udg`, `udg!`.
  \
  \ }doc

[unneeded] default-udg-chars ?( need rom-font need get-udg

rom-font 'A' 8 * +    \ from
get-udg @ 144 8 * +   \ to
'U' 'A' - 1+ 8 *      \ count
move ?)

  \ doc{
  \
  \ default-udg-chars ( -- )
  \
  \ A phoney word used only to do ``need default-udg-chars`` in
  \ order to define UDG 144..164 as letters 'A'..'U', copied
  \ from the ROM font, the shape they have in Sinclair BASIC by
  \ default. The current value of `os-udg` is used.
  \
  \ WARNING: In Solo Forth `os-udg` points to bitmap of UDG
  \ 0, while in Sinclair BASIC it points to bitmap of UDG
  \ 144.
  \
  \ See also: `block-chars`, `set-udg`, `rom-font`.
  \
  \ }doc

[unneeded] udg> ?( need /udg* need get-udg

: udg> ( n -- a ) /udg* get-udg + ; ?)

  \ doc{
  \
  \ udg> ( c -- a )
  \
  \ Convert UDG number _n_ (0..255) to the address _a_ of its
  \ bitmap, pointed by `os-udg`.
  \
  \ See also: `udg!`, `udg:`, `/udg*`, `get-udg`.
  \
  \ }doc

[unneeded] udg! ?( need udg>

: udg! ( b0..b7 c -- ) udg> dup 7 + ?do i c! -1 +loop ; ?)

  \ doc{
  \
  \ udg! ( b0..b7 c -- )
  \
  \ Store the 8-byte bitmap _b0..b7_ into UDG _c_ (0..255) of
  \ the UDG font pointed by `os-udg`.  _b0_ is the first (top)
  \ scan.  _b7_ is the last (bottom) scan.
  \
  \ See also: `udg:`, `udg>`.
  \
  \ }doc

[unneeded] udg: ?( need udg! ?(

: udg: ( b0..b7 c "name" -- ) dup cconstant udg! ; ?)

  \ doc{
  \
  \ udg: ( b0..b7 c "name" -- )
  \
  \ Create a `cconstant` _name_ for UDG char _c_ (0..255) and
  \ store the 8-byte bitmap _b0..b7_ into that UDG char.  _b0_
  \ is the first (top) scan.  _b7_ is the last (bottom) scan.
  \
  \ See also: `udg!`, `udg>`.
  \
  \ }doc

( udg[ )

need get-udg need binary

variable udg0  variable current-udg  variable current-scan
  \ XXX TODO -- rename (to prevent name clashes) and document

: udg[ ( b -- )
  dup udg0 !  current-udg !  current-scan off  binary ;

  \ doc{
  \
  \ udg[ ( c -- )
  \
  \ Start a set of UDG definitions, from UDG character _c_
  \ (0..255).
  \
  \ Usage example:
  \
  \ ----
  \ 140 udg[  \ define UDG 140..144
  \
  \ 00111100 | 00111100 | 00111100 | 00111100 | 00111100 ||
  \ 01111110 | 01111110 | 01111110 | 01111110 | 01011110 ||
  \ 11111111 | 11111111 | 11111111 | 10111111 | 10111111 ||
  \ 11111111 | 11111111 | 10111111 | 10111111 | 11111111 ||
  \ 11111111 | 10111111 | 10111111 | 11111111 | 11111111 ||
  \ 11001111 | 11011111 | 11111111 | 11111111 | 11111111 ||
  \ 01111110 | 01111110 | 01111110 | 01111110 | 01111110 ||
  \ 00111100 | 00111100 | 00111100 | 00111100 | 00111100 ||]
  \ ----
  \
  \ See also: `|`, `||`, `||]`.
  \
  \ }doc

: | ( b -- ) get-udg current-udg @ 8 * current-scan @ + + c!
               1 current-udg +! ;

  \ doc{
  \
  \ | ( b -- )
  \
  \ Store scan _b_ into the current UDG being defined.
  \
  \ See also: `udg[`, `||`, `||]`.
  \
  \ }doc

: || ( b -- ) |  1 current-scan +!  udg0 @ current-udg ! ;

  \ doc{
  \
  \ || ( b -- )
  \
  \ Store scan _b_ into the current UDG being defined and start
  \ a new row of scans.
  \
  \ See also: `udg[`, `|`, `||]`.
  \
  \ }doc


: ||] ( b -- ) ||  decimal ;

  \ doc{
  \
  \ ||] ( b -- )
  \
  \ Store scan _b_ into the current UDG being defined and stop
  \ defining UDGs.
  \
  \ See also: `udg[`, `|`, `||`.
  \
  \ }doc

( udg-scan>number )

need binary

create udg-blank '.' c,  create udg-dot 'X' c,

  \ doc{
  \
  \ udg-blank  ( -- ca )
  \
  \ A character variable that holds the characted used by
  \ `grid` and `g` as a grid blank. By default it's '.'.
  \
  \ See also: `udg-dot`, `udg-scan>binary`.
  \
  \ }doc

  \ doc{
  \
  \ udg-dot  ( -- ca )
  \
  \ A character variable that holds the characted used by
  \ `grid` and `g` as a grid blank. By default it's 'X'.
  \
  \ See also: `udg-blank`, `udg-scan>binary`.
  \
  \ }doc

: udg-scan>binary ( ca len -- )
  bounds ?do i c@ dup udg-blank c@ =
                  if   drop '0' i c!
                  else udg-dot c@ = if '1' i c! then
                  then loop ;

  \ doc{
  \
  \ udg-scan>binary ( ca len -- )
  \
  \ Convert the characters `udg-blank` and `udg-dot` found in
  \ UDG scan string _ca len_ to '0' and '1' respectively.
  \
  \ }doc

: udg-scan>number? ( ca len -- n true | false )
  2dup udg-scan>binary base @ >r binary number? r> base ! ;

  \ doc{
  \
  \ udg-scan>number? ( ca len -- n true | false )
  \
  \ Is UDG scan string _ca len_ a valid binary number?
  \ The string is processed by `udg-scan>binary` first.
  \
  \ See also: `udg-scan>number`.
  \
  \ }doc

: udg-scan>number ( ca len -- n )
  >stringer udg-scan>number? 0= #-290 ?throw ;

  \ doc{
  \
  \ udg-scan>number ( ca len -- n )
  \
  \ If UDG scan string _ca len_, after being processed by
  \ `udg-scan>binary`, is a valid binary number, return the
  \ result _n_.  Otherwise throw exception #-290 (invalid UDG
  \ scan).
  \
  \ See also: `udg-scan>number?`, `udg-block`.
  \
  \ }doc

( parse-udg-block-row )

  \ XXX UNDER DEVELOPMENT -- 2017-03-19
  \
  \ A possible factor of `udg-block` to skip invalid UDG scans,
  \ in order to allow UDG blocks span on several Forth blocks,
  \ ignoring the index line.
  \
  \ But anyway this is not needed when `load-app` is used.

need parse-name-thru

: parse-udg-block-row ( "name..." -- ca len )
  base @ >r
  begin
    begin parse-name-thru 2dup >stringer 2dup udg-scan>binary
          evaluate
    while
  while repeat r> base ! ; -->


: parse-udg-block-row ( len "name..." -- ca len )
  begin
    begin dup parse-name-thru rot over <> while 2drop repeat
    2dup
  while repeat ; -->

( udg-block )

need udg-scan>number need udg> need /udg need /udg*
need udg-width need parse-name-thru need j need anon

here anon> ! 3 cells allot

: udg-block ( width height c "name..." -- )
  3 set-anon
    \ Set the anonymous local variables:
    \   [ 0 ] anon = _c_
    \   [ 1 ] anon = height
    \   [ 2 ] anon = width
  [ 1 ] anon @ /udg* 0 ?do parse-name-thru ( ca len )
    [ 2 ] anon @ 0 ?do
      over udg-width udg-scan>number ( ca len b )
      j /udg /mod [ 2 ] anon @ * i + /udg* +
        \ Calculate the offset from the address of _c_ in
        \ the UDG font, to store the scan _b_.
      [ 0 ] anon @ udg> + c!
        \ Store _b_ at the proper address in the UDG font,
        \ i.e. the address of _c_ plus the offset.
      udg-width /string ( ca' len' ) loop 2drop loop ;

  \ doc{
  \
  \ udg-block ( width height c "name..." -- )
  \
  \ Start a set of UDG definitions that form a UDG block, from
  \ UDG character _c_ (0..255). _width_ and _height_ are in
  \ characters.  The maximum _width_ is 7 (imposed by the size
  \ of Forth source blocks). _height_ has no maximum, as the
  \ UDG block can ocuppy more than one Forth block (provided
  \ the Forth block have no index line, i.e. `load-app` is used
  \ to load the source).

  \ Usage example:
  \
  \ ----
  \ 5 2 140 udg-block
  \
  \ 0011110000111100001111000011110000111100
  \ 0111111001111110011111100111111001011110
  \ 1111111111111111111111111011111110111111
  \ 1111111111111111101111111011111111111111
  \ 1111111110111111101111111111111111111111
  \ 1100111111011111111111111111111111111111
  \ 0111111001111110011111100111111001111110
  \ 0011110000111100001111000011110000111100
  \ 0011110000111100001111000011110000111100
  \ 0111111001111110011111100111111001011110
  \ 1111111111111111111111111011111110111111
  \ 1111111111111111101111111011111111111111
  \ 1111111110111111101111111111111111111111
  \ 1100111111011111111111111111111111111111
  \ 0111111001111110011111100111111001111110
  \ 0011110000111100001111000011110000111100
  \ ----
  \
  \ }doc

( udg-row[ )

need get-udg need evaluate need binary need abort"
need parse-name-thru

8 constant udg-height  8 constant udg-width
  \ height in bytes (scans)
  \ width in pixels

variable udg-row-height  variable udg-row-width
  \ height in scans
  \ width in characters

variable udg-row-first-udg

: ?block-scan-length ( n -- )
  dup udg-width mod abort" Wrong block scan length"
  udg-width / udg-row-width @ ?dup
  if    <> abort" Wrong block width"
          \ not the first scan, so check the width
  else  udg-row-width !  then ;
          \ first scan, so save the width

: udg-row-current-row ( -- n )
  udg-row-height @ udg-height / ;

: udg-current-scan ( -- n )
  udg-row-height @ udg-height mod ;  -->

( udg-row[ )

: >udg-scan ( n -- a )
  udg-height * udg-current-scan +
  udg-row-first-udg @ udg-height * +  get-udg + ;
  \ Convert column _n_ of the current UDG row to address _a_
  \ of the scan of the current UDG.

: udg-scan! ( b n -- ) >udg-scan c! ;
  \ Store UDG scan _b_, which is at column _n_ of the current UDG
  \ block.

: udg-row-scan ( ca len -- )
  base @ >r binary  dup ?block-scan-length
  dup udg-width / 0 ?do  over udg-width
    evaluate i udg-scan!  udg-width /string
  loop  2drop  r> base !  1 udg-row-height +! ;
  \ Manage a UDG row scan _ca len_, extracting the individual
  \ UDG scans from it.

: ]udg-row ( ca len -- )
  2drop  udg-row-height @ udg-height <>
  abort" The height of the UDG row is wrong" ;
  \ End a UDG row. Check its height.

: udg-row-scan? ( ca len -- f ) s" ]udg-row" compare 0<> ;
  \ Is the string _ca len_ a UDG row scan
  \ instead of the end of the UDG row?

-->

( udg-row[ )

: udg-row[ ( c "ccc" -- )
  udg-row-first-udg !  udg-row-height off  udg-row-width off
  begin   parse-name-thru 2dup udg-row-scan?
  while   udg-row-scan
  repeat  ]udg-row ;

  \ doc{
  \
  \ udg-row[ ( c "ccc" -- )
  \
  \ Start a UDG row (a graphic formed by a row of UDG). Parse
  \ its scans, extract the individual UDG scans and store them
  \ starting from UDG code _c_ (0..255).

  \ Usage example:
  \
  \ ----
  \ 140 udg-row[
  \
  \ 0011110000111100001111000011110000111100
  \ 0111111001111110011111100111111001011110
  \ 1111111111111111111111111011111110111111
  \ 1111111111111111101111111011111111111111
  \ 1111111110111111101111111111111111111111
  \ 1100111111011111111111111111111111111111
  \ 0111111001111110011111100111111001111110
  \ 0011110000111100001111000011110000111100
  \
  \ ]udg-row
  \ ----
  \
  \ }doc

( make-block-chars )

need assembler

code make-block-chars ( a -- )
  h pop, b push,
  #128 a ld#,  \ first char is #128
  rbegin
    af push, a b ld, 0B3B call, af pop, a inc,
  #144 cp#, nz? runtil  \ last char is #143
  b pop, jpnext, end-code

  \ Note: $0B3B is a secondary entry point to the PO-GR-1 ROM
  \ routine ($0B38), in order to force a non-default value of
  \ the HL register, which holds the destination address.

  \ doc{
  \
  \ make-block-chars ( a -- )
  \
  \ Make the bit patterns of the 16 ZX Spectrum block
  \ characters, originally assigned to character codes
  \ 128..143, and store them (128 bytes in total) from address
  \ _a_.
  \
  \ ``make-block-chars`` is provided for easier conversion of
  \ BASIC programs that use the original block characters.
  \ These characters are part of the ZX Spectrum character set,
  \ but they are not included in the ROM font. Instead, their
  \ bitmaps are built on the fly by the BASIC ROM printing
  \ routine. In Solo Forth there's no such restriction, and
  \ characters 0..255 can be redefined by the user.
  \
  \ ``make-block-chars`` is written in Z80 and uses 18 B of
  \ code space, but the word `block-chars` is provided as an
  \ alternative.
  \
  \ }doc

( block-chars )

$0F $0F $0F $0F $00 $00 $00 $00 #129 need udg! udg!
$F0 $F0 $F0 $F0 $00 $00 $00 $00 #130 udg!
$FF $FF $FF $FF $00 $00 $00 $00 #131 udg!
$00 $00 $00 $00 $0F $0F $0F $0F #132 udg!
$0F $0F $0F $0F $0F $0F $0F $0F #133 udg!
$F0 $F0 $F0 $F0 $0F $0F $0F $0F #134 udg!
$FF $FF $FF $FF $0F $0F $0F $0F #135 udg!
$00 $00 $00 $00 $F0 $F0 $F0 $F0 #136 udg!
$0F $0F $0F $0F $F0 $F0 $F0 $F0 #137 udg!
$F0 $F0 $F0 $F0 $F0 $F0 $F0 $F0 #138 udg!
$FF $FF $FF $FF $F0 $F0 $F0 $F0 #139 udg!
$00 $00 $00 $00 $FF $FF $FF $FF #140 udg!
$0F $0F $0F $0F $FF $FF $FF $FF #141 udg!
$F0 $F0 $F0 $F0 $FF $FF $FF $FF #142 udg! need udg>
$FF $FF $FF $FF $FF $FF $FF $FF #143 udg! #128 udg> 8 erase

  \ Note: In this block `need` is in unusual places. The goal
  \ is to save one line and make the 16 character definitions
  \ fit one block while keeping the layout clean.  At the end
  \ `erase` creates char #128, which is blank.

  \ doc{
  \
  \ block-chars ( -- )
  \
  \ A phoney word used only to do ``need block-chars``.  The
  \ loading of the correspondent source block will define
  \ characters 128..143 as block characters, with the shape
  \ they have in Sinclair BASIC.  The current value of `os-udg`
  \ is used.
  \
  \ See also: `make-block-chars`, `set-udg`, `udg!`,
  \ `default-udg-chars`.
  \
  \ }doc

( set-udg get-udg )

[unneeded] set-udg ?( need os-udg

code set-udg ( a -- ) E1 c, 22 c, os-udg , jpnext, end-code ?)
  \ pop hl
  \ ld (sys_udg),hl
  \ jp next

  \ doc{
  \
  \ set-udg ( a -- )
  \
  \ Set address _a_ as the the current UDG set (characters
  \ 0..255), by changing the system variable `os-udg`.  _a_
  \ must be the bitmap address of character 0.
  \
  \ See also: `get-udg`, `set-font`.
  \
  \ }doc

[unneeded] get-udg ?( need os-udg

code get-udg ( -- a ) 2A c, os-udg , jppushhl, end-code ?)
  \ ld hl, (sys_udg)
  \ jp pushhl

  \ doc{
  \
  \ get-udg ( -- a )
  \
  \ Get address _a_ of the current UDG set (characters
  \ 0..255), by fetching the system variable `os-udg`.  _a_
  \ is the bitmap address of character 0.
  \
  \ See also: `set-udg`.
  \
  \ }doc

( xy>scra_ xy>scra )

[unneeded] xy>scra_ ?( need assembler

create xy>scra_ ( -- a ) asm

  b a ld, %11000 and#, #64 add#, a d ld, b a ld, %111 and#,
  rrca, rrca, rrca, a e ld, c a ld, e add, a e ld,
  ret, end-asm ?)

  \ ld a,b     ; y coordinate
  \ and %11000 ; mask segment: 0..2
  \ add a,64   ; 64*256 = 16384, Spectrum's screen memory
  \ ld d,a     ; high byte of the screen address
  \ ld a,b     ; y coordinate
  \ and %111   ; mask row within segment: 0..7
  \ rrca       ; multiply row...
  \ rrca       ; ...
  \ rrca       ; ...by 32
  \ ld e,a     ; low byte of the screen address
  \ ld a,c     ; add on y coordinate
  \ add a,e    ; mix with low byte
  \ ld e,a     ; DE = address of screen position
  \ ret

  \ Credit:
  \
  \ How To Write ZX Spectrum Games – Chapter 9
  \ http://chuntey.arjunnair.in/?p=154

  \ doc{
  \
  \ xy>scra_ ( -- a )
  \
  \ Return address _a_ of a Z80 routine that calculates the
  \ screen address correspondent to given cursor coordinates.
  \
  \ Input registers:
  \
  \ - B = y coordinate (0..23)
  \ - C = x coordinate (0..31)
  \
  \ Output registers:
  \
  \ - DE = screen address
  \
  \ See also: `xy>scra`, `gxy>scra_`.
  \
  \ }doc

[unneeded] xy>scra ?( need assembler need xy>scra_

code xy>scra ( x y -- a )

  h pop, l a ld, h pop, b push, a b ld, l c ld,
  xy>scra_ call, exde, b pop, jppushhl, end-code ?)

  \ pop hl
  \ ld a,l
  \ pop hl
  \ push bc          ; save Forth IP
  \ ld b,a           ; y coordinate
  \ ld c,l           ; x coordinate
  \ call cursor_addr
  \ ex de,hl         ; HL = screen address
  \ pop bc           ; restore Forth IP
  \ jp push_hl

  \ doc{
  \
  \ xy>scra ( x y -- a )
  \
  \ Convert cursor coordinates _x y_ to their correspondent
  \ screen address _a_.
  \
  \ See also: `xy>scra_` , `gxy>scra`.
  \
  \ }doc

( xy>scra_ xy>scra )

[unneeded] xy>scra_ ?( need assembler

create xy>scra_ ( -- a ) asm

  \ XXX TMP --
  \
  \ Alternative version that does not use the BC register and
  \ returns the address in HL.

  d a ld, %11000 and#, #64 add#, a h ld, d a ld, %111 and#,
  rrca, rrca, rrca, a l ld, e a ld, l add, a l ld,
  ret, end-asm ?)

  \ ld a,d     ; y coordinate
  \ and %11000 ; mask segment: 0..2
  \ add a,64   ; 64*256 = 16384, Spectrum's screen memory
  \ ld h,a     ; high byte of the screen address
  \ ld a,d     ; y coordinate
  \ and %111   ; mask row within segment: 0..7
  \ rrca       ; multiply row...
  \ rrca       ; ...
  \ rrca       ; ...by 32
  \ ld l,a     ; low byte of the screen address
  \ ld a,e     ; add on y coordinate
  \ add a,l    ; mix with low byte
  \ ld l,a     ; HL = address of screen position
  \ ret

  \ Credit:
  \
  \ How To Write ZX Spectrum Games – Chapter 9
  \ http://chuntey.arjunnair.in/?p=154

  \ xy>scra_ ( -- a )
  \
  \ Return address _a_ of a Z80 routine that calculates the
  \ screen address correspondent to given cursor coordinates.
  \
  \ Input registers:
  \
  \ - D = y coordinate (0..23)
  \ - E = x coordinate (0..31)
  \
  \ Output registers:
  \
  \ - HL = screen address
  \
  \ See also: `xy>scra`, `gxy>scra_`.

[unneeded] xy>scra ?( need assembler need xy>scra_

code xy>scra ( x y -- a )

  h pop, d pop, l d ld, xy>scra_ call, jppushhl,
  end-code ?)

  \ pop hl           ; L = y coordinate
  \ pop de           ; E = x coordinate
  \ ld d,l           ; D = y coordinate
  \ call cursor_addr
  \ jp push_hl

  \ xy>scra ( x y -- a )
  \
  \ Convert cursor coordinates _x y_ to their correspondent
  \ screen address _a_.
  \
  \ See also: `xy>scra_` , `gxy>scra`.

( display-char-bitmap_ )

[unneeded] display-char-bitmap_ ?(

need assembler need xy>scra_

create display-char-bitmap_ ( -- a ) asm

  \ ; Input:
  \ ;   HL = address of the character bitmap
  \ ;   B = y coordinate (0..23)
  \ ;   C = x coordinate (0..31)

  xy>scra_ call, 8 b ld#,

  \   call cursor_addr ; DE = screen address
  \   ld b,8 ; pixels high

  rbegin  m a ld, d stap, h incp, d inc,  rstep ret, end-asm ?)

  \ display_char_row:
  \   ld a,(hl)             ; row of graphic bitmap
  \   ld (de),a             ; transfer to screen
  \   inc hl                ; next row of graphic bitmap
  \   inc d                 ; next pixel line
  \   djnz display_char_row
  \   ret

  \ doc{
  \
  \ display-char-bitmap_ ( -- a )
  \
  \ Return address _a_ of a Z80 routine that displays
  \ the bitmap of a character at given cursor coordinates.
  \
  \ Input registers:
  \
  \ - HL = address of the character bitmap
  \ - B = y coordinate (0..23)
  \ - C = x coordinate (0..31)
  \
  \ }doc

( display-char-bitmap_ )

  \ XXX TMP --
  \
  \ Alternative version that uses the version of
  \ `xy>scra_` that does not use the BC register.

[unneeded] display-char-bitmap_ ?(

need assembler need xy>scra_

create display-char-bitmap_ ( -- a ) asm

  \ ; Input:
  \ ;   HL = address of the character bitmap
  \ ;   D = y coordinate (0..23)
  \ ;   E = x coordinate (0..31)

  xy>scra_ call,

  \   push hl
  \   call cursor_addr      ; HL = screen address
  \   ex de,hl
  \   pop hl

  8 b ld#, rbegin  m a ld, d stap, h incp, d inc,  rstep  ret,

  \   ld b,8                ; pixels high
  \ display_char_row:
  \   ld a,(hl)             ; row of graphic bitmap
  \   ld (de),a             ; transfer to screen
  \   inc hl                ; next row of graphic bitmap
  \   inc d                 ; next pixel line
  \   djnz display_char_row
  \   ret

  end-asm ?)

  \ display-char-bitmap_ ( -- a )
  \
  \ Return address _a_ of a Z80 routine that displays
  \ the bitmap of a character at given cursor coordinates.
  \
  \ Input registers:
  \
  \ - HL = address of the character bitmap
  \ - B = y coordinate (0..23)
  \ - C = x coordinate (0..31)

( at-xy-display-udg udg-at-xy-display )

  \ XXX UNDER DEVELOPMENT

[unneeded] at-xy-display-udg ?(

need assembler need display-char-bitmap_ need os-udg

unused code at-xy-display-udg ( c x y -- )

  h pop, l a ld, h pop, a d ld, l e ld,

  \ pop hl                    ; L = y coordinate
  \ ld a,l                    ; A = y coordinate
  \ pop hl                    ; L = x coordinate
  \ ld d,a                    ; D = y coordinate
  \ ld e,l                    ; E = x coordinate

  h pop, b push, d b ld, e c ld, h addp, h addp, h addp,
  os-udg d ftp, d addp, display-char-bitmap_ call,

  \ pop hl                    ; HL = _c_ (0..255), H must be 0
  \ push bc                   ; save Forth IP
  \ ld b,d                    ; B = y coordinate
  \ ld c,e                    ; C = y coordinate
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl                 ; multiply HL by 8 (8 scans per character)
  \ ld de,(sys_udg)
  \ add hl,de                 ; HL = address of the bitmap of _c_
  \ call display_char_bitmap

  b pop, jpnext, end-code unused - cr u. ?)

  \ pop bc                    ; restore Forth IP
  \ _jp_next

  \ XXX REMARK -- 23 bytes used (without requirements)

  \ doc{
  \
  \ at-xy-display-udg ( c x y -- )
  \
  \ Display UDG _c_ at cursor coordinates _x y_. This is much
  \ faster than using `at-xy` and `emit-udg`, because no ROM
  \ routine is used, the cursor coordinates are not updated and
  \ the screen attributtes are not changed (only the character
  \ bitmap is displayed).
  \
  \ See also: `udg-at-xy-display`.
  \
  \ }doc

[unneeded] udg-at-xy-display ?(

need assembler need display-char-bitmap_ need os-udg

unused code udg-at-xy-display ( x y c -- )

  h pop, h addp, h addp, h addp, os-udg d ftp, d addp,

  \ pop hl          ; HL = _c_ (0..255), H must be 0
  \ add hl,hl
  \ add hl,hl
  \ add hl,hl       ; multiply HL by 8 (8 scans per character)
  \ ld de,(sys_udg)
  \ add hl,de       ; HL = address of the bitmap of _c_

  d pop, e a ld, d pop, b push, a b ld, e c ld,
  display-char-bitmap_ call,

  \ pop de                    ; E = y coordinate
  \ ld a,e                    ; A = y coordinate
  \ pop de                    ; E = x coordinate
  \ push bc                   ; save Forth IP
  \ ld b,a                    ; B = y coordinate
  \ ld c,e                    ; C = x coordinate
  \ call display_char_bitmap

  b pop, jpnext, end-code unused - cr u. ?)

  \ pop bc                    ; restore Forth IP
  \ _jp_next

  \ XXX REMARK -- 21 bytes used (without requirements)

  \ doc{
  \
  \ udg-at-xy-display ( x y c -- )
  \
  \ Display UDG _c_ (0..255) at cursor coordinates _x y_. This
  \ is much faster than a combination of `at-xy` and
  \ `emit-udg`, because no ROM routine is used, the cursor
  \ coordinates are not updated and the screen attributtes are
  \ not changed (only the character bitmap is displayed).
  \
  \ See also: `at-xy-display-udg`.
  \
  \ }doc

( type-udg )

: type-udg ( ca len -- ) bounds ?do i c@ emit-udg loop  ;

  \ doc{
  \
  \ type-udg ( ca len -- )
  \
  \ If _len_ is greater than zero, display the UDG character
  \ string _ca len_. All characters of the string are printed
  \ with `emit-udg`.
  \
  \ See also: `type`.
  \
  \ }doc

( grid g )

need binary need abort"

create grid-blank '.' c,  create grid-dot 'X' c,

  \ doc{
  \
  \ grid-blank  ( -- ca )
  \
  \ A character variable that holds the characted used by
  \ `grid` and `g` as a grid blank. By default it's '.'.
  \
  \ See also: `grid-dot`.
  \
  \ }doc

  \ doc{
  \
  \ grid-dot  ( -- ca )
  \
  \ A character variable that holds the characted used by
  \ `grid` and `g` as a grid blank. By default it's 'X'.
  \
  \ See also: `grid-blank`.
  \
  \ }doc

: grid>binary ( ca len -- )
  bounds ?do i c@ dup grid-blank c@ =
                  if   drop '0' i c!
                  else grid-dot c@ = if '1' i c! then
                  then loop ;

: grid>number? ( ca len -- n true | false )
  2dup grid>binary base @ >r binary number? r> base ! ;

: grid-row ( ca len -- n )
  >stringer grid>number? 0= abort" Invalid UDG grid" ; -->

( grid g )

: grid ( "name..." )
  begin  begin parse-name dup while 2dup s" end-grid" str=
                                    if 2drop exit then grid-row
         repeat 2drop
  refill 0= until ;

  \ doc{
  \
  \ grid  ( "name..." -- )
  \
  \ Parse a UDG grid and convert it to numbers, until
  \ ``end-grid`` is found.  The grid can be written with binary
  \ numbers (without the "%" prefix") or with the characters
  \ stored at `grid-blank` and `grid-dot`.
  \
  \ NOTE: At the moment the maximum length of the grid rows is
  \ 16 pixels. Only the 16 pixels at the right of the row are
  \ used.
  \
  \ Usage example:

  \ ----
  \ grid
  \ ......XXXX......
  \ ...XXXXXXXXXX...
  \ ..XXXXXXXXXXXX..
  \ ..XXX..XX..XXX..
  \ ..XXXXXXXXXXXX..
  \ .....XX..XX.....
  \ ....XX.XX.XX....
  \ ..XX........XX..
  \ end-grid
  \ ----

  \ See also: `g`.
  \
  \ }doc

: g ( "name" -- ) parse-name grid-row ;

  \ doc{
  \
  \ g  ( "name" -- n | d )
  \
  \ Parse a UDG grid row and convert it to number.  The grid
  \ row can be written with binary numbers (without the "%"
  \ prefix") or with the characters stored at `grid-blank` and
  \ `grid-dot`.
  \
  \ NOTE: At the moment the maximum length of the grid rows is
  \ 16 pixels. Only the 16 pixels at the right of the row are
  \ used.
  \
  \ Usage example:

  \ ----
  \ g ......XXXX......
  \ g ...XXXXXXXXXX...
  \ g ..XXXXXXXXXXXX..
  \ g ..XXX..XX..XXX..
  \ g ..XXXXXXXXXXXX..
  \ g .....XX..XX.....
  \ g ....XX.XX.XX....
  \ g ..XX........XX..
  \ ----

  \ See also: `grid`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-23: Add `0udg:`. Factor `0udg!` from `udg!`.
  \ Improve the documentation.
  \
  \ 2016-04-24: Add `udg[` and `0udg[`.
  \
  \ 2016-10-11: Add `udg-row[`.
  \
  \ 2016-12-21: Add `make-block-characters`,
  \ `block-characters`, `0udg>`, `set-udg`, `get-udg`,
  \ `set-font`, `get-font`, `rom-font`.
  \
  \ 2016-12-22: Rename "-characters" to "-chars" in all words,
  \ to be consistent with the convention used in the escaped
  \ strings module.
  \
  \ 2016-12-23: Add `udg-chars`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-09: Rename `udg-chars` to `default-udg-chars`. Make
  \ the basic words individually accessible to `need`.  Improve
  \ documentation.
  \
  \ 2017-01-09: Add `(cursor-addr)` and `cursor-addr`.  Add
  \ temporary experimental words `(display-char-bitmap)`,
  \ `at-xy-display-0udg` and `0udg-at-xy-display`.
  \
  \ 2017-01-10: Improve documentation.
  \
  \ 2017-01-11: Move `set-font` to the kernel, because `cold`
  \ must set the default font.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-22: Rewrite `get-udg`, `set-udg` and `get-font` in
  \ Z80.
  \
  \ 2017-02-02: Use `need binary` instead of `[undefined]
  \ binary`.
  \
  \ 2017-02-04: Adapt to 0-index-only UDG, after the changes in
  \ the kernel: Remove `first-udg`. Remove `udg!` and rename
  \ `0udg!` to `udg!`; remove 'udg:` and rename `0udg:` to
  \ `udg:`; rename `0udg>` to `udg>`; change the calculation of
  \ `default-udg-chars`; remove `udg[` and rename `0udg[` to
  \ `udg[`;  rename `at-xy-display-0udg` to
  \ `at-xy-display-udg`; rename `0udg-at-xy-display` to
  \ `udg-at-xy-display`. Compact the code, saving one block.
  \ Improve `udg>` and other words ẁith `get-udg`.
  \
  \ 2017-02-16: Deactivate documentation of alternative
  \ implementations of `(cursor-addr)`, `cursor-addr` and
  \ `(display-char-bitmap)`, to prevent them from being
  \ included twice in the glossary.
  \
  \ 2017-02-17: Update cross references.  Change markup of
  \ inline code that is not a cross reference.
  \
  \ 2017-02-19: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-24: Fix requirement of `udg-row[`. Fix block char
  \ 128.
  \
  \ 2017-02-25: Add `type-udg`.
  \
  \ 2017-02-27: Move `get-font` and `rom-font` to the new
  \ module <printing.fonts.fs>.
  \
  \ 2017-03-13: Improve documentation.  Update references:
  \ `(pixel-addr)` to `gxy>scra_`, `pixel-addr` to `gxy>scra`.
  \ Rename: `cursor-addr` to `xy>scra`, `(cursor-addr)` to
  \ `xy>scra_`, `(display-char-bitmap)` to
  \ `display-char-bitmap_`.
  \
  \ 2017-03-17: Add `grid`, `end-grid`, `g`... Another tool to
  \ define UDG grids, before combining all of them into one
  \ single tool with all the features.
  \
  \ 2017-03-18: Start implementation of `udg-block`, which will
  \ supersede `grid` and `udg-row[`.
  \
  \ 2017-03-19: Add `/udg`, `/udg*`, `udg-width`. Finish
  \ `udg-block`.

  \ vim: filetype=soloforth
