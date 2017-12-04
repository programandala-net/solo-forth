  \ menu.sinclair.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201711252216
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Configurable Sinclair-style menus.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- Factor the Sinclair style to an upper layer, in
  \ order to make the menu configurable with any style.
  \
  \ XXX TODO -- Make it possible to configure key lists instead
  \ of single keys.
  \
  \ XXX TODO -- Use a module to hide private words.

( menu )

need attr! need xy>attra need get-udg need set-udg
need type-left-field need case need array>
need white need black need cyan need papery need brighty
need overprint-off need inverse-off
need xy>gxy need ortholine need 8*
need under+ need within need polarity

-->

( menu )

create sinclair-stripes-bitmaps ( -- a )

$01 c, $03 c, $07 c, $0F c, $1F c, $3F c, $7F c, $FF c,

  \ 0 0 0 0 0 0 0 1           X
  \ 0 0 0 0 0 0 1 1          XX
  \ 0 0 0 0 0 1 1 1         XXX
  \ 0 0 0 0 1 1 1 1        XXXX
  \ 0 0 0 1 1 1 1 1       XXXXX
  \ 0 0 1 1 1 1 1 1      XXXXXX
  \ 0 1 1 1 1 1 1 1     XXXXXXX
  \ 1 1 1 1 1 1 1 1    XXXXXXXX

$FE c, $FC c, $F8 c, $F0 c, $E0 c, $C0 c, $80 c, $00 c,

  \ 1 1 1 1 1 1 1 0    XXXXXXX
  \ 1 1 1 1 1 1 0 0    XXXXXX
  \ 1 1 1 1 1 0 0 0    XXXXX
  \ 1 1 1 1 0 0 0 0    XXXX
  \ 1 1 1 0 0 0 0 0    XXX
  \ 1 1 0 0 0 0 0 0    XX
  \ 1 0 0 0 0 0 0 0    X
  \ 0 0 0 0 0 0 0 0

here dup $10 c, $02 c, $80 c, $11 c, $06 c, $81 c,
         $10 c, $04 c, $80 c, $11 c, $05 c, $81 c,
         $10 c, $00 c, $80 c,
here - abs 2constant sinclair-stripes$ ( -- ca len )

  \ The compiled `sinclair-stripes$` string consists of the
  \ following codes:

  \ $10 $02 = set ink 2 (red)
  \ $80     = first stripe UDG
  \ $11 $06 = set paper 6 (yellow)
  \ $81     = second stripe UDG
  \ $10 $04 = set ink 4 (green)
  \ $80     = first stripe UDG
  \ $11 $05 = set paper 5 (cyan)
  \ $81     = second stripe UDG
  \ $10 $00 = set ink 0 (black)
  \ $80     = first stripe UDG

: .sinclair-stripes ( -- )
  get-udg
  [ sinclair-stripes-bitmaps 128 8 * - ] literal set-udg
  sinclair-stripes$ type set-udg ;

5 cconstant /stripes
  \ Size of the stripes on the display, in characters.

-->

( menu )

2variable menu-xy

2variable menu-title

variable actions-table

variable options-table

create menu-width 0 c, create menu-options 0 c,

create menu-banner-attr black papery white + brighty c,

create menu-body-attr white papery brighty c,

create menu-key-down '6' c,

create menu-key-up   '7' c,

create menu-key-choose 13 c,

create menu-highlight-attr cyan papery brighty c,

variable menu-rounding  menu-rounding on

-->

( menu )

: .banner ( -- )
  menu-banner-attr c@ attr! overprint-off inverse-off
  menu-xy 2@
  2dup at-xy menu-title 2@ menu-width c@ type-left-field
       swap menu-width c@ + [ /stripes 1+ ] cliteral - swap
       at-xy .sinclair-stripes ;
  \ Display the banner of the current menu.

: (.option ( ca len -- ) menu-width c@ 1- type-left-field ;
  \ Display menu option _ca len_ at the current cursor
  \ coordinates.

: option>xy ( n -- x y ) menu-xy 2@ rot + 1+ ;
  \ Convert menu option _n_ to its cursor coordinates _x y_.

: at-option ( n -- ) option>xy at-xy ;
  \ Set the cursor at option _n_.

-->

( menu )

: vertical-line ( gx gy -- )
  0 -1 menu-options c@ 1+ 8* ortholine ;
  \ Draw a vertical 1-pixel border from _gx gy_ down to
  \ the bottom of the menu.

: menu-x-pixels ( -- n ) menu-width c@ 8* ;

: .border ( -- )
  menu-xy 2@ 1+
  2dup xy>gxy 2dup menu-x-pixels 1- under+ vertical-line
                                           vertical-line
       menu-options c@ + 1+ xy>gxy 1+ 1 0 menu-x-pixels
       ortholine ;
  \ Draw a 1-pixel border around the menu options, preserving
  \ the attributes.

  \ XXX TODO -- Reuse the result of the first `xy>gyx` for the
  \ horizontal line, the calculation will be faster.

-->

( menu )

: .option ( n -- )
  dup at-option space options-table @ array> @ count (.option ;
  \ Display menu option _n_ of the current menu.

: .options ( -- )
  menu-body-attr c@ attr!
  menu-options c@ dup 0 ?do i .option loop
                      at-option menu-width c@ spaces ;
  \ Display the options of the current menu, from texts table
  \ _a_.

: option>attrs ( n -- ca len )
  option>xy xy>attra menu-width c@ ;
  \ Convert menu option _n_ to its attributes zone _ca len_.

create current-option 0 c,

: -option ( -- ) current-option c@
                 option>attrs menu-body-attr c@ fill ;
  \ Remove the highlighting of the current option.

: +option ( n -- ) dup current-option c!
                   option>attrs menu-highlight-attr c@ fill ;
  \ Set _n_ as the current option and highlight it.

-->

( menu )

: round-option ( n -- n' )
  dup 0 menu-options c@ within ?exit
      polarity ( -1|1) 0< ( -1|0) menu-options c@ 1- and ;

: limit-option ( n -- n' ) 0 max menu-options c@ 1- min ;

: adjust-option ( n -- n' )
  menu-rounding @ if   round-option exit
                  then limit-option ;

: option+ ( n -- ) current-option c@ + adjust-option +option ;
  \ Add _n_ to the current option, make the result fit the
  \ valid range and make it the current option.

: previous-option ( -- ) -option -1 option+ ;

: next-option     ( -- ) -option  1 option+ ;

: choose-option ( n1 -- )
  current-option c@ actions-table @ array> perform ; -->

( menu )

: menu ( -- )
  0 dup current-option c! +option
  begin key case
          menu-key-up     c@ of previous-option   endof
          menu-key-down   c@ of next-option       endof
          menu-key-choose c@ of choose-option     endof
        endcase again ;

  \ doc{
  \
  \ menu  ( -- )
  \
  \ Activate the current menu, which has been set by `set-menu`
  \ and displayed by `.menu`.
  \
  \ See also: `new-menu`.
  \
  \ }doc

: .menu ( -- ) .banner .options .border ;

  \ doc{
  \
  \ .menu  ( -- )
  \
  \ Display the current menu, which has been set by `set-menu`
  \ and can be activated by `menu`.
  \
  \ See also: `new-menu`.
  \
  \ }doc

: set-menu ( a1 a2 ca len x y n1 n2 -- )
  menu-options c! [ /stripes 2+ ] cliteral max menu-width c!
  menu-xy 2! menu-title 2!  options-table ! actions-table ! ;

  \ doc{
  \
  \ new-menu ( a1 a2 ca len x y n1 n2 -- )
  \
  \ Set the current menu to cursor coordinates _x y_,
  \ _n2_ options, _n1_ characters width, title _ca len_,
  \ actions table _a1_ (a cell array of _n2_ execution tokens)
  \ and option texts table _a2_ (a cell array of _n2_ addresses
  \ of counted strings).
  \
  \ See also: `new-menu`, `.menu`, `menu`.
  \
  \ }doc

: new-menu ( a1 a2 ca len x y n1 n2 -- ) set-menu .menu menu ;

  \ doc{
  \
  \ new-menu ( a1 a2 ca len x y n1 n2 -- )
  \
  \ Set, display at cursor coordinates _x y_ and activate a new
  \ menu of _n2_ options, _n1_ characters width, title _ca
  \ len_, actions table _a1_ (a cell array of _n2_ execution
  \ tokens) and option texts table _a2_ (a cell array of _n2_
  \ addresses of counted strings).
  \
  \ See also: `set-menu`, `.menu`, `menu`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-03-28: Start. First working version.
  \
  \ 2017-03-29: Add the 1-pixel border. Add `menu-rounding` to
  \ configure the behaviour of the option selector.
  \
  \ 2017-11-25: Update stack comments.

  \ vim: filetype=soloforth
