  \ menu.sinclair.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007112244
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Configurable Sinclair-style menus.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018, 2020.

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

( sinclair-stripes sinclair-stripes$ .sinclair-stripes )

unneeding sinclair-stripes ?(

create sinclair-stripes ( -- ca )

$01 c, $03 c, $07 c, $0F c, $1F c, $3F c, $7F c, $FF c,

  \ 0 0 0 0 0 0 0 1           X
  \ 0 0 0 0 0 0 1 1          XX
  \ 0 0 0 0 0 1 1 1         XXX
  \ 0 0 0 0 1 1 1 1        XXXX
  \ 0 0 0 1 1 1 1 1       XXXXX
  \ 0 0 1 1 1 1 1 1      XXXXXX
  \ 0 1 1 1 1 1 1 1     XXXXXXX
  \ 1 1 1 1 1 1 1 1    XXXXXXXX

$FE c, $FC c, $F8 c, $F0 c, $E0 c, $C0 c, $80 c, $00 c, ?)

  \ 1 1 1 1 1 1 1 0    XXXXXXX
  \ 1 1 1 1 1 1 0 0    XXXXXX
  \ 1 1 1 1 1 0 0 0    XXXXX
  \ 1 1 1 1 0 0 0 0    XXXX
  \ 1 1 1 0 0 0 0 0    XXX
  \ 1 1 0 0 0 0 0 0    XX
  \ 1 0 0 0 0 0 0 0    X
  \ 0 0 0 0 0 0 0 0

  \ doc{
  \
  \ sinclair-stripes ( -- ca )
  \
  \ Return address _ca_ where the following pair of UDG
  \ definitions, used to create Sinclair stripes, are stored:

  \ ....
  \ 0 0 0 0 0 0 0 1           X
  \ 0 0 0 0 0 0 1 1          XX
  \ 0 0 0 0 0 1 1 1         XXX
  \ 0 0 0 0 1 1 1 1        XXXX
  \ 0 0 0 1 1 1 1 1       XXXXX
  \ 0 0 1 1 1 1 1 1      XXXXXX
  \ 0 1 1 1 1 1 1 1     XXXXXXX
  \ 1 1 1 1 1 1 1 1    XXXXXXXX
  \
  \ 1 1 1 1 1 1 1 0    XXXXXXX
  \ 1 1 1 1 1 1 0 0    XXXXXX
  \ 1 1 1 1 1 0 0 0    XXXXX
  \ 1 1 1 1 0 0 0 0    XXXX
  \ 1 1 1 0 0 0 0 0    XXX
  \ 1 1 0 0 0 0 0 0    XX
  \ 1 0 0 0 0 0 0 0    X
  \ 0 0 0 0 0 0 0 0
  \ ....

  \ See: `.sinclair-stripes`,  `sinclair-stripes$`.
  \
  \ }doc

unneeding sinclair-stripes$ ?(

here dup $10 c, $02 c, $80 c, $11 c, $06 c, $81 c,
         $10 c, $04 c, $80 c, $11 c, $05 c, $81 c,
         $10 c, $00 c, $80 c,
here - abs 2constant sinclair-stripes$ ( -- ca len ) ?)

  \ doc{
  \
  \ sinclair-stripes$ ( -- ca len )
  \
  \ Return a string _ca len_ containing the following character
  \ codes:

  \ .Characters of ``sinclair-stripes$``.
  \ |===
  \ | Code(s) | Meaning
  \
  \ | $10 $02 | set ink 2 (red)
  \ | $80     | first stripe UDG
  \ | $11 $06 | set paper 6 (yellow)
  \ | $81     | second stripe UDG
  \ | $10 $04 | set ink 4 (green)
  \ | $80     | first stripe UDG
  \ | $11 $05 | set paper 5 (cyan)
  \ | $81     | second stripe UDG
  \ | $10 $00 | set ink 0 (black)
  \ | $80     | first stripe UDG
  \ |===

  \ Definitions for UDG codes $80 and $81 are provided
  \ optionally by `sinclair-stripes`.
  \
  \ See: `.sinclair-stripes`.
  \
  \ }doc

unneeding .sinclair-stripes ?( need sinclair-stripes
                               need sinclair-stripes$

: .sinclair-stripes ( -- )
  get-udg [ sinclair-stripes 128 8 * - ] literal set-udg
  sinclair-stripes$ type set-udg ; ?)

  \ doc{
  \
  \ .sinclair-stripes ( -- ) "dot-sinclair-stripes"
  \
  \ Display the Sinclair stripes by using `sinclair-stripes` as
  \ UDG font and typing `sinclair-stripes$`. The current UDG
  \ font is preserved.
  \
  \ See: `set-udg`, `get-udg`.
  \
  \ }doc

5 cconstant /sinclair-stripes

  \ doc{
  \
  \ /sinclair-stripes ( -- len )
  \
  \ A `cconstant`. _len_ is the size of `sinclair-stripes$` in
  \ graphic characters, i.e. the visible length of the string
  \ when displayed.
  \
  \ ``/sinclair-stripes`` is used by `set-menu` and other menu
  \ words.
  \
  \ }doc

( menu )

need attr! need xy>attra need get-udg need set-udg
need type-left-field need case need array>
need white need black need cyan need papery need brighty
need overprint-off need inverse-off
need xy>gxy need ortholine need 8*
need under+ need within need polarity
need .sinclair-stripes need 2variable

-->

( menu )

2variable menu-xy

  \ doc{
  \
  \ menu-xy ( -- a ) "menu-x-y"
  \
  \ A `2variable`. _a_ is the address of a double cell
  \ containing the coordinates (column and row) of the current
  \ `menu`. ``menu-xy`` is set by `set-menu`.
  \
  \ See: `menu-width`, `.menu-border`, `.menu-banner`.
  \
  \ }doc

2variable menu-title

  \ doc{
  \
  \ menu-title ( -- a )
  \
  \ A `2variable`. _a_ is the address of a double cell
  \ containing the address and length of a string which is the
  \ title of the current `menu`. ``menu-title`` is set by
  \ `set-menu`.
  \
  \ See: `menu-width`, `menu-xy`, `menu-banner-attr`,
  \ `.menu-banner`.
  \
  \ }doc

variable actions-table

  \ doc{
  \
  \ actions-table ( -- a )
  \
  \ A `variable`, _a_ is the address of a cell containing the
  \ address of a cell array which holds the execution tokens of
  \ the current `menu` options. ``actions-table`` is set by
  \ `set-menu`.
  \
  \ See: `options-table`.
  \
  \ }doc

variable options-table

  \ doc{
  \
  \ options-table ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of a cell array, which holds the counted strings of
  \ the current `menu` options. ``options-table`` is set by
  \ `set-menu`.
  \
  \ See: `actions-table`.
  \
  \ }doc

create menu-width 0 c, create menu-options 0 c,

  \ doc{
  \
  \ menu-width ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ width of the current `menu` in characters. ``menu-width``
  \ is set by `set-menu`.
  \
  \ See: `menu-title`, `menu-body-attr`, `menu-banner-attr`,
  \ `menu-highlight-attr`.
  \
  \ }doc

  \ doc{
  \
  \ menu-options ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ current `menu` number of options. ``menu-options`` is set
  \ by `set-menu`.
  \
  \ See: `menu-width`.
  \
  \ }doc

create menu-banner-attr black papery white + brighty c,

  \ doc{
  \
  \ menu-banner-attr ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ attribute of the current `menu` banner. Its default value
  \ is white ink on black paper, with bright.
  \
  \ See: `menu-body-attr`, `menu-highlight-attr`,
  \ `.menu-banner`, `black`, `papery`, `white`, `brighty`.
  \
  \ }doc

create menu-body-attr white papery brighty c,

  \ doc{
  \
  \ menu-body-attr ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ attribute of the current `menu` background. Its default
  \ value is black ink on white paper, with bright.
  \
  \ See: `menu-banner-attr`, `menu-highlight-attr`,
  \ `.menu-options`, `white`, `papery`, `brighty`.
  \
  \ }doc

create menu-key-down '6' c,

  \ doc{
  \
  \ menu-key-down ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ key code used to move the current `menu` selection down.
  \ Its default value is character '6'.
  \
  \ See: `menu-key-up`, `menu-key-choose`.
  \
  \ }doc

create menu-key-up   '7' c,

  \ doc{
  \
  \ menu-key-up ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ key code used to move the current `menu` selection up.
  \ Its default value is character '7'.
  \
  \ See: `menu-key-down`, `menu-key-choose`.
  \
  \ }doc

create menu-key-choose 13 c,

  \ doc{
  \
  \ menu-key-choose ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ key code used to move the current `menu` selection down.
  \ Its default value is 13, i.e. the enter key.
  \
  \ See: `menu-key-up`, `menu-key-down`.
  \
  \ }doc

create menu-highlight-attr cyan papery brighty c,

  \ doc{
  \
  \ menu-highlight-attr ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a byte containing the
  \ attribute used to highlight the current `menu` option. Its
  \ default value is the combination of `cyan`, `papery` and
  \ `brighty`, i.e. black ink on cyan bright paper.
  \
  \ See: `menu-banner-attr`.
  \
  \ }doc

variable menu-rounding  menu-rounding on

  \ doc{
  \
  \ menu-rounding ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing a
  \ flag. When the flag is non-zero, the top and the bottom
  \ `menu` options are connected in a circular manner, i.e.
  \ pressing `menu-key-up` at the top option leads to to the
  \ botton option, and pressing `menu-key-down` at the bottom
  \ option lead to the top.
  \
  \ See: `menu-key-choose`, `menu-highlight-attr`.
  \
  \ }doc

-->

( menu )

: .menu-banner ( -- )
  menu-banner-attr c@ attr! overprint-off inverse-off
  menu-xy 2@
  2dup at-xy menu-title 2@ menu-width c@ type-left-field
       swap menu-width c@ +
       [ /sinclair-stripes 1+ ] cliteral - swap
       at-xy .sinclair-stripes ;

  \ doc{
  \
  \ .menu-banner ( -- ) "dot-menu-banner"
  \
  \ Display the banner of the current `menu`.
  \
  \ See: `menu-banner-attr`, `menu-title`, `menu-width`,
  \ `.sinclair-stripes`, `.menu`, `.menu-options`,
  \ `.menu-border`, `type-left-field`, `menu-xy`.
  \
  \ }doc

: (.option ( ca len -- ) menu-width c@ 1- type-left-field ;
  \ Display menu option _ca len_ at the current cursor
  \ coordinates.

: option>xy ( n -- col row ) menu-xy 2@ rot + 1+ ;
  \ Convert menu option _n_ to its cursor coordinates _col row_.

: at-option ( n -- ) option>xy at-xy ;
  \ Set the cursor at option _n_.

-->

( menu )

: vertical-line ( gx gy -- )
  0 -1 menu-options c@ 1+ 8* ortholine ;
  \ Draw a vertical 1-pixel border from _gx gy_ down to
  \ the bottom of the menu.

: menu-x-pixels ( -- n ) menu-width c@ 8* ;

: .menu-border ( -- )
  menu-xy 2@ 1+
  2dup xy>gxy 2dup menu-x-pixels 1- under+ vertical-line
                                           vertical-line
       menu-options c@ + 1+ xy>gxy 1+ 1 0 menu-x-pixels
       ortholine ;

  \ doc{
  \
  \ .menu-border ( -- ) "dot-menu-border"
  \
  \ Draw a 1-pixel border around the current `menu` options,
  \ preserving the attributes.
  \
  \ See: `.menu`, `.menu-options`, `.menu-banner`, `ortholine`,
  \ `menu-xy`, `xy>gxy`.
  \
  \ }doc

  \ XXX TODO -- Reuse the result of the first `xy>gyx` for the
  \ horizontal line, the calculation will be faster.

-->

( menu )

: .menu-option ( n -- )
  dup at-option space options-table @ array> @ count (.option ;

  \ doc{
  \
  \ .menu-option ( n -- ) "dot-menu-option"
  \
  \ Display menu option _n_ of the current `menu`.
  \
  \ See: `.menu-options`, `.menu`.
  \
  \ }doc

: .menu-options ( a -- )
  menu-body-attr c@ attr!
  menu-options c@ dup 0 ?do i .menu-option loop
                      at-option menu-width c@ spaces ;

  \ doc{
  \
  \ .menu-options ( -- ) "dot-menu-options"
  \
  \ Display the options of the current `menu`.
  \
  \ See: `.menu`, `.menu-option`, `.menu-border`,
  \ `.menu-banner`.
  \
  \ }doc

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
  0 +option
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
  \ See: `new-menu`,
  \ `menu-key-up`, `menu-key-down`, `menu-key-choose`,
  \ `options-table`, `actions-table`.
  \
  \ }doc

: .menu ( -- ) .menu-banner .menu-options .menu-border ;

  \ doc{
  \
  \ .menu  ( -- ) "dot-menu"
  \
  \ Display the current menu, which has been set by `set-menu`
  \ and can be activated by `menu`.
  \
  \ See: `new-menu`, `.menu-banner`, `.menu-options`,
  \ `.menu-border`.
  \
  \ }doc

: set-menu ( a1 a2 ca len col row n1 n2 -- )
  menu-options c!
  [ /sinclair-stripes 2+ ] cliteral max menu-width c!
  menu-xy 2! menu-title 2!  options-table ! actions-table ! ;

  \ doc{
  \
  \ set-menu ( a1 a2 ca len col row n1 n2 -- )
  \
  \ Set the current menu to cursor coordinates _col row_, _n2_
  \ options, _n1_ characters width, title _ca len_, actions
  \ table _a1_ (a cell array of _n2_ execution tokens) and
  \ option texts table _a2_ (a cell array of _n2_ addresses of
  \ counted strings).
  \
  \ See: `new-menu`, `.menu`, `menu`, `menu-xy`, `menu-title`,
  \ `actions-table`, `menu-options`, `menu-width`,
  \ `menu-body-attr`, `menu-highlight-attr`,
  \ `menu-banner-attr`.
  \
  \ }doc

: new-menu ( a1 a2 ca len col row n1 n2 -- )
  set-menu .menu menu ;

  \ doc{
  \
  \ new-menu ( a1 a2 ca len col row n1 n2 -- )
  \
  \ Set, display an activate a new menu at cursor coordinates
  \ _col row_, with _n2_ options, _n1_ characters width, title
  \ _ca len_, actions table _a1_ (a cell array of _n2_
  \ execution tokens) and option texts table _a2_ (a cell array
  \ of _n2_ addresses of counted strings).
  \
  \ Usage example:

  \ ----

  \ need menu need :noname
  \
  \ :noname ( -- ) unnest unnest ;
  \ :noname ( -- ) 2 border ;
  \ :noname ( -- ) 1 border ;
  \ :noname ( -- ) 0 border ;
  \
  \ create actions> , , , ,
  \
  \ here s" EXIT"  s,
  \ here s" Red"   s,
  \ here s" Blue"  s,
  \ here s" Black" s,
  \
  \ create texts> , , , ,
  \
  \ : menu-pars ( -- a1 a2 ca len col row n1 n2 )
  \   actions> texts> s" Border" 7 7 14 4 ;
  \
  \ menu-pars new-menu

  \ ----

  \ See: `set-menu`, `.menu`, `menu`.
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
  \
  \ 2018-03-07: Add words' pronunciaton. Improve documentation.
  \
  \ 2018-03-08: Rename `sinclair-stripes-bitmaps`
  \ `sinclair-stripes`. Make it, `sinclair-stripes$` and
  \ `.sinclair-stripes` independent, reusable. Fix
  \ documentation.
  \
  \ 2018-03-09: Update stack notation "x y" to "col row".
  \
  \ 2018-07-25: Rename `/stripes` to `/sinclair-stripes` and
  \ document it.
  \
  \ 2018-07-27: Remove useless code from `menu`. Improve
  \ documentation of `new-menu`.
  \
  \ 2020-05-05: Fix typo in cross reference.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library.
  \
  \ 2020-05-24: Rename `.option`, `.options`, `.border` and
  \ `.banner` to `.menu-option`, `.menu-options`,
  \ `.menu-border` and `.menu-banner`. Increase and improve
  \ documentation.
  \
  \ 2020-07-11: Add title to table.

  \ vim: filetype=soloforth
