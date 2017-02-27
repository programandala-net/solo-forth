  \ strings.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702272345

  \ -----------------------------------------------------------
  \ Description

  \ Misc words related to strings.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-04-22: Add `s""`, moved from the kernel.
  \
  \ 2016-04-24: Add `need pick`, because `pick` has been moved
  \ from the kernel to the library.
  \
  \ 2016-04-27: Add `char-in-string?` and `char-position?`.
  \
  \ 2016-05-05: Rename `s=` to `str=`. Add `str<` and `str>`.
  \
  \ 2016-05-11: Fix `-prefix`. Start compacting the blocks.
  \
  \ 2016-08-02: Improve comment of `hunt`. Comment the string
  \ comparison operators.
  \
  \ 2016-08-05: Reorganize and compact the code to save one
  \ block.
  \
  \ 2016-11-17: Fix needing `trim`. Fix `+place`.
  \
  \ 2016-11-19: Update credit of `prefix?`.
  \
  \ 2016-11-21: Complete and improve documentation. Move
  \ `contains` from <tools.list.blocks.fsb> here.
  \
  \ 2016-11-25: Rewrite `lengths` in Z80. Make `lengths` and
  \ `s+` independently accessible for `need`.
  \
  \ 2016-12-06: Move `>cell-string` from the `switch` structure
  \ module.
  \
  \ 2016-12-07: Rename `>cell-string` to `>bstring`. Add
  \ `c>bstring`, `2>bstring`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2016-12-16: Add `sconstants`, `/sconstants`, `u>str`.
  \
  \ 2016-12-18: Add `uppers1`. Make `#chars` and `#spaces`
  \ individually accesible to `need`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2016-12-30: Move `s'` here from its own module.
  \
  \ 2017-01-09: Add `fars,`, `farsconstant`, `farsconstants`,
  \ `/farsconstants`.
  \
  \ 2017-01-10: Add `far,"`,  `far>sconstants`,
  \ `/far>sconstants`, `save-farstring`.
  \
  \ 2017-01-10: Move all far-memory strings words to their own
  \ module <strings.far.fsb>.  Simplify the string arrays:
  \ remove the variant `sconstants`, which doesn't leave the
  \ count on the stack, and rename `/sconstants` to
  \ `sconstants`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-21: Add `/counted-string`.
  \
  \ 2017-01-22: Add `unescape`.
  \
  \ 2017-02-01: Fix needing of `#spaces` and `#chars`. Move
  \ `upper` and `uppers` from the kernel. Add `upper-routine`.
  \ Move `lowers` from the kernel.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-22: Move `str=` to the kernel, because `?(` has
  \ been moved too.
  \
  \ 2017-02-27: Improve documentation.

( str< str> trim +place hunt )

[unneeded] str<
?\ : str< ( ca1 len1 ca2 len2 -- f ) compare 0< ;

  \ doc{
  \
  \ str< ( ca1 len1 ca2 len2 -- f )
  \
  \ Is string _ca1 len1_ lexicographically smaller than string
  \ _ca2 len2_?
  \
  \ See also: `str=`, `str>`, `compare`.
  \
  \ }doc

[unneeded] str>
?\ : str> ( ca1 len1 ca2 len2 -- f ) compare 0> ;

  \ doc{
  \
  \ str> ( ca1 len1 ca2 len2 -- f )
  \
  \ Is string _ca1 len1_ lexicographically larger than string
  \ _ca2 len2_?
  \
  \ See also: `str<`, `str=`, `compare`.
  \
  \ }doc

[unneeded] trim
?\ : trim ( ca1 len1 -- ca2 len2 ) -leading -trailing ;

  \ doc{
  \
  \ trim ( ca1 len1 -- ca1' len1' )
  \
  \ Remove leading and trailing spaces from a string _ca len1_,
  \ returning string _ca1' len1'_.
  \
  \ See also: `-leading`, `-trailing`.
  \
  \ }doc

[unneeded] +place ?( need c+!
: +place ( ca1 len1 ca2 -- )
  2dup 2>r count + smove 2r> c+! ; ?)

  \ doc{
  \
  \ +place ( ca1 len1 ca2 -- )
  \
  \ Add the string _ca1 len1_ to the end of the counted string
  \ _ca2_.
  \
  \ See also: `place`, `s+`, `smove`, `count`.
  \
  \ }doc

[unneeded] hunt ?(
: hunt ( ca1 len1 ca2 len2 -- ca3 len3 )
  search 0= if  chars + 0  then ; ?)

  \ Credit:
  \
  \ Code from Wil Baden's Charscan library (2003-02-17),
  \ public domain.

  \ doc{
  \
  \ hunt ( ca1 len1 ca2 len2 -- ca3 len3 )
  \
  \ Search a string _ca1 len1_ for a substring _ca2 len2_.
  \ Return the part of _ca1 len1_ that starts with the first
  \ occurence of _ca2 len2_. Therefore _ca3 len3_ = _ca1+n
  \ len1-n_.
  \
  \ Origin: Charscan library, by Wil Baden, 2003-02-17, public
  \ domain.
  \
  \ See also: `search`, `compare`, `skip`, `scan`.
  \
  \ }doc

( /counted-string ud>str u>str d>str char>string chars>string )

[unneeded] /counted-string ?\ 255 cconstant /counted-string
  
  \ doc{
  \
  \ /counted-string  ( -- len )
  \
  \ _len_ is the maximum length of a counted string, in
  \ characters.
  \
  \ See also: `environment?`.
  \
  \ }doc

[unneeded] ud>str
?\ : ud>str ( ud -- ca len ) <# #s #> ;

  \ Credit:
  \
  \ Code from Galope (module ud-to-str.fs).

  \ doc{
  \
  \ ud>str ( ud -- ca len )
  \
  \ Convert _ud_ to string _ca len_.
  \
  \ See also: `u>str`, `d>str`, `char>string`.
  \
  \ }doc

[unneeded] u>str
?\ need ud>str  : u>str ( u -- ca len ) s>d ud>str ;

  \ doc{
  \
  \ u>str ( u -- ca len )
  \
  \ Convert _u_ to string _ca len_.
  \
  \ See also: `ud>str`, `d>str`, `char>string`.
  \
  \ }doc

[unneeded] d>str ?(
: d>str ( d -- ca len ) tuck dabs <# #s rot sign #> ; ?)

  \ Credit:
  \
  \ Code from Galope (module d-to-str.fs).

  \ doc{
  \
  \ d>str ( d -- ca len )
  \
  \ Convert _d_ to string _ca len_.
  \
  \ See also: `u>str`, `ud>str`, `char>string`.
  \
  \ }doc

[unneeded] char>string ?(
: char>string ( c -- ca len )
  1 allocate-string tuck c! 1 ; ?)

  \ XXX TODO -- rename or write after `c>bstring`, which
  \ does the same but in `pad`.

  \ doc{
  \
  \ char>string ( c -- ca len )
  \
  \ Convert the char _c_ to a string _ca len_ in the
  \ `stringer`.
  \
  \ See also: `chars>string`, `ruler`, `u>str`, `d>str`,
  \ `ud>str`.
  \
  \ }doc

[unneeded] chars>string ?(
: chars>string ( c1..cn n -- ca len )
  dup if    dup allocate-string swap 2dup 2>r ( c1..cn ca n )
            bounds ?do  i c!  loop  2r>
      else  pad swap  then ; ?)

  \ doc{
  \
  \ chars>string ( c1..cn n -- ca len )
  \
  \ Convert _n_ chars to a string _ca len_ in the `stringer`.
  \
  \ c1..cn :: chars to make the string with (_c1_ is the last one)
  \ n :: number of chars
  \
  \ See also: `ruler`, `s+`.
  \
  \ }doc

( >bstring c>bstring 2>bstring )

[unneeded] >bstring
?\ : >bstring ( u -- ca len ) pad ! pad cell ;

  \ doc{
  \
  \ >bstring ( x -- ca len )
  \
  \ Convert _x_ to a 1-cell binary string _ca len_ in `pad`.
  \ _ca len_ contains _x_ "as is", as stored in memory.
  \
  \ See also: `c>bstring`, `2>bstring`.
  \
  \ }doc

[unneeded] c>bstring
?\ : c>bstring ( c -- ca len ) pad c! pad 1 ;

  \ doc{
  \
  \ c>bstring ( c -- ca len )
  \
  \ Convert _c_ to a 1-char binary string _ca len_ in `pad`,
  \ _ca len_ contains _c_ "as is", as stored in memory.
  \
  \ See also: `>bstring`, `2>bstring`.
  \
  \ }doc

[unneeded] 2>bstring  ?(
: 2>bstring ( xd -- ca len )
  pad 2! pad [ 2 cells ] literal ; ?)

  \ doc{
  \
  \ >2bstring ( xd -- ca len )
  \
  \ Convert _xd_ to a 2-cell binary string in `pad`.
  \ _ca len_ contains _xd_ "as is", as stored in memory.
  \
  \ See also: `>bstring`, `c>bstring`.
  \
  \ }doc

( lengths s+ )

[unneeded] lengths ?(

code lengths
  \ ( ca1 len1 ca2 len2 -- ca1 len1 ca2 len2 len1 len2 )

  \ 11 bytes in Forth:

    \   : lengths  2over nip over ;

  \ 12 bytes in Z80:

  D9 c, E1 c, D1 c, C1 c, C5 c, D5 c, E5 c, C5 c, E5 c, D9 c,
  jpnext, end-code ?)
    \         ; T   B
    \         ; --- --
    \ exx     ;  04 01
    \ pop hl  ;  10 01
    \ pop de  ;  10 01
    \ pop bc  ;  10 01
    \ push bc ;  10 01
    \ push de ;  11 01
    \ push hl ;  11 01
    \ push bc ;  11 01
    \ push hl ;  11 01
    \ exx     ;  04 01
    \ _jpnext ;  08 02 `jp (ix)`
    \         ; --- --
    \         ; 100 12 Total

  \ Credit:
  \
  \ Code adapted from Afera.

  \ doc{
  \
  \ lengths ( ca1 len1 ca2 len2 -- ca1 len1 ca2 len2 len1 len2 )
  \
  \ Duplicate lengths _len1_ and _len2_ of strings _ca1 len1_
  \ and _ca2 len2_. This word is a factor of `s+`.
  \
  \ This word is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : lengths ( ca1 len1 ca2 len2 -- ca1 len1 ca2 len2 len1 len2 )
  \   2over nip over ;
  \ ----

  \ }doc

[unneeded] s+  ?( need lengths need pick

: s+ ( ca1 len1 ca2 len2 -- ca3 len3 )
  lengths + >r          ( ca1 len2 ca2 len2 ) ( r: len3 )
  r@ allocate-string >r ( r: len3 ca3 )
  2 pick r@ +           ( ca1 len1 ca2 len2 len1+ca3 )
  smove                 ( ca1 len1 ) \ 2nd string to buffer
  r@ smove              \  1st string to buffer
  r> r> ; ?)

  \ Credit:
  \
  \ Code adapted from Afera.

  \ doc{
  \
  \ s+ ( ca1 len1 ca2 len2 -- ca3 len3 )
  \
  \ Append the string _ca2 len2_ to the end of string _ca1
  \ len1_ returning the string _ca3 len3_ in the `stringer`.
  \
  \ See also: `/string`, `string/`, `lengths`.
  \
  \ }doc

( upper upper-routine uppers uppers1 )

[unneeded] upper [unneeded] upper-routine and ?( 0

code upper ( c -- c' ) E1 c, 7D c, 21 c, pusha , E5 c,
  \   pop hl
  \   ld a,l
  \   ld hl,push_a ; exit address
  \   push hl ; make next `ret` jump to push_a
  drop here \ upper_routine:
  \ ; Convert the ASCII char in the 'A' register to uppercase.
  FE c, 'a' c, D8 c, FE c, 'z' 1+ c, D0 c, E6 c, %11011111 c,
  C9 c, end-code
  \   cp 'a'
  \   ret c
  \   cp 'z'+1
  \   ret nc
  \   and %11011111
  \   ret

  \ XXX TODO convert to deferred, to let it be customized for
  \ 8-bit charsets.

  \ doc{
  \
  \ upper ( c -- c' )
  \
  \ Convert _c_ to uppercase _c'_.
  \
  \ See also: `uppers`, `lower`, `upper-routine`.
  \
  \ }doc

get-current swap assembler-wordlist set-current

constant upper-routine ( -- a )

set-current ?)

  \ doc{
  \
  \ upper-routine ( -- a )
  \
  \ Return address _a_ of a routine that converts the ASCII
  \ character in the A register to uppercase.
  \
  \ See also: `upper`, `lower-routine`.
  \
  \ }doc

[unneeded] uppers ?( need upper-routine

code uppers ( ca len -- )
  D1 c, E1 c, here 7A c, B3 c, CA c, next , 7E c,
  \   pop de ; E = string length
  \   pop hl ; string address
  \ uppers.do:
  \   ld a,d
  \   or e
  \   jp z,next
  \   ld a,(hl)
  upper-routine call, 77 c, 23 c, 1B c, C3 c, , end-code ?)
  \   call upper_routine
  \   ld (hl),a
  \   inc hl
  \   dec de
  \   jp uppers.do

  \ doc{
  \
  \ uppers ( ca len -- )
  \
  \ Convert string _ca len_ to uppercase.
  \
  \ See also: `uppers1`, `lowers`, `upper`.
  \
  \ }doc

[unneeded] uppers1
?\ need uppers  : uppers1 ( ca len -- ) drop 1 uppers ;

  \ doc{
  \
  \ uppers1 ( ca len -- )
  \
  \ Change the first char of string _ca len_ to uppercase.
  \
  \ See also: `uppers`, `upper`.
  \
  \ }doc

( lowers )

code lowers ( ca len -- )
  D1 c, E1 c, here 7A c, B3 c, CA c, next , 7E c,
  \   pop de ; E = string length
  \   pop hl ; string address
  \ lowers.do:
  \   ld a,d
  \   or e
  \   jp z,next
  \   ld a,(hl)
  lower-routine call, 77 c, 23 c, 1B c, C3 c, , end-code
  \   call lower_routine
  \   ld (hl),a
  \   inc hl
  \   dec de
  \   jp lowers.do

  \ doc{
  \
  \ lowers ( ca len -- )
  \
  \ Convert string _ca len_ to lowercase.
  \
  \ See also: `uppers`, `lower`.
  \
  \ }doc

( #spaces #chars )

[unneeded] #spaces ?( need under+

  \ XXX TODO -- finish and benchmark. rewrite `#spaces` after
  \ `#chars`? add `#nulls`

: #spaces ( ca len -- +n )
  0 rot rot  0 ?do  count bl = under+  loop  drop abs ; ?)

  \ Credit:
  \
  \ Code improved from:
  \ http://forth.sourceforge.net/mirror/comus/index.html

  \ XXX UNDER DEVELOPMENT

  \ doc{
  \
  \ #spaces ( ca len -- +n )
  \
  \ Count number _+n_ of spaces in a string _ca len_.
  \
  \ See also: `#chars`, `spaces`.
  \
  \ }doc

[unneeded] #chars ?( need under+
: #chars ( ca len c -- +n )
  0 2swap 0 ?do
    ( c count ca ) count over = under+  loop  2drop abs ; ?)

  \ doc{
  \
  \ #chars ( ca len c -- +n )
  \
  \ Return the count _+n_ of chars _c_ in a string _ca len_.
  \
  \ See also: `#spaces`, `char-in-string?`, `char-position?`.
  \
  \ }doc

( /name first-name last-name )

[unneeded] last-name ?( need trim

: last-name ( ca1 len1 -- ca2 len2 )
  trim  begin  2dup bl scan bl skip dup
        while  2nip  repeat  2drop ; ?)

  \ Credit:
  \
  \ Code from Galope.

  \ doc{
  \
  \ last-name ( ca1 len1 -- ca2 len2 )
  \
  \ Get the last name _ca2 len2_ from string _ca1 len1_.  A
  \ name is a substring separated by spaces.
  \
  \ See also: `first-name`, `/name`, `string/`, `-suffix`.
  \
  \ }doc

: /name ( ca1 len1 -- ca2 len2 ca3 len3 )
  bl skip 2dup bl scan ;

  \ Credit:
  \
  \ Code from Galope.

  \ doc{
  \
  \ /name ( ca1 len1 -- ca2 len2 ca3 len3 )
  \
  \ Split string _ca1 len1_ into _ca2 len2_ (from the start of
  \ the first name in _ca1 len1_) and _ca3 len3_ (from the char
  \ after the first name in _ca1 len1).  A name is a substring
  \ separated by spaces.
  \
  \ See also: `first-name`, `/string`, `-prefix`, `-suffix`,
  \ `string/`.
  \
  \ }doc

[unneeded] first-name ?exit

: first-name ( ca1 len1 -- ca2 len2 ) /name nip - ;

  \ Credit:
  \
  \ Code from Galope.

  \ doc{
  \
  \ first-name ( ca1 len1 -- ca2 len2 )
  \
  \ Return the first name _ca2 len2_ from string _ca1 len1_.  A
  \ name is a substring separated by spaces.
  \
  \ See also: `last-name`, `/name`, `-prefix`, `/string`.
  \
  \ }doc

( prefix? suffix? -prefix -suffix )

[unneeded] prefix?

?\ : prefix? ( ca1 len1 ca2 len2 -- f ) tuck 2>r min 2r> str= ;

  \ Credit:
  \
  \ Code from Gforth's `string-prefix?` (compat/strcomp.fs),
  \ public domain.

  \ doc{
  \
  \ prefix? ( ca1 len1 ca2 len2 -- f )
  \
  \ Is string _ca2 len2_ the prefix of string _ca1 len1_?
  \
  \ See also: `suffix?`, `-prefix`.
  \
  \ }doc

[unneeded] suffix? (? need pick

: suffix? ( ca1 len1 ca2 len2 -- f )
  2swap dup 3 pick - /string str= ; ?)

  \ Credit:
  \
  \ Code from Galope (module string-suffix-question.fs).

  \ doc{
  \
  \ suffix? ( ca1 len1 ca2 len2 -- f )
  \
  \ Is string _ca2 len2_ the suffix of string _ca1 len1_?
  \
  \ See also: `-suffix`, `prefix?`.
  \
  \ }doc

[unneeded] -prefix ?( need prefix?

: -prefix ( ca1 len1 ca2 len2 -- ca1 len1 | ca3 len3 )
  dup >r 2over 2swap prefix?
  if  swap r@ + swap r> -  else  rdrop  then ; ?)

  \ Credit:
  \
  \ Code from Galope (module minus-prefix.fs).

  \ doc{
  \
  \ -prefix ( ca1 len1 ca2 len2 -- ca1 len1 | ca3 len3 )
  \
  \ Remove prefix _ca2 len2_ from string _ca1 len1_.
  \
  \ See also: `-suffix`, `/string`, `1/string`, `-leading`.
  \
  \ }doc

[unneeded] -suffix ?( need suffix?

: -suffix ( ca1 len1 ca2 len2 -- ca1 len1 | ca3 len3 )
  dup >r 2over 2swap suffix?
  if  r> -  else  rdrop  then ; ?)

  \ Credit:
  \
  \ Code from Galope (module minus-suffix.fs).

  \ doc{
  \
  \ -suffix ( ca1 len1 ca2 len2 -- ca1 len1 | ca3 len3 )
  \
  \ Remove suffix _ca2 len2_ from string _ca1 len1_.
  \
  \ See also: `-prefix`, `string/`, `chop`, `-trailing`.
  \
  \ }doc

( contains chop s"" sconstant save-counted-string s' )

[unneeded] contains ?\ : contains  search nip nip ;
                         \ ( ca1 len1 ca2 len2 -- f )

  \ doc{
  \
  \ contains ( ca1 len1 ca2 len2 -- f )
  \
  \ Does string _ca1 len1_ contain string _ca2 len2_?
  \
  \ See also: `char-position?`, `char-in-string?`, `compare`,
  \ `#chars`,
  \
  \ }doc

  \ `contains` is defined also in <002.need.fsb>, where it is
  \ needed, but evidently it's not accessible there, because
  \ `[unneeded]` is not defined yet at that point.

[unneeded] chop
?\ : chop ( ca len -- ca' len' ) 1- swap char+ swap ;

  \ Credit:
  \
  \ Code from Galope (module chop.fs).

  \ doc{
  \
  \ chop ( ca len -- ca' len' )
  \
  \ Remove the last character from string _ca len_.
  \
  \ See also: `-suffix`, `/string`, `string/`.
  \
  \ }doc

[unneeded] s"" ?\ : s"" ( -- ca len ) 0 allocate-string 0 ;

  \ doc{
  \
  \ s"" ( -- ca len )
  \
  \ Return an empty string in the circular string buffer.
  \
  \ See also: `s"`, `s\"`, `s'`.
  \
  \ }doc

[unneeded] s' ?\ : s'  ''' parse-string ; immediate
  \ ( Compilation: "ccc<char>" -- ) ( Run-time:  -- ca len )

  \ Credit:
  \
  \ Code from Afera.

  \ doc{
  \
  \ s' ( Compilation: "ccc<char>" -- ) ( Run-time:  -- ca len )
  \
  \ Identical to the standard word `s"`, but using single
  \ quote as delimiter. A simple alternative to `s\"` when only
  \ double quotes are needed in a string.
  \
  \ ``s'`` is an `immediate` word.
  \
  \ }doc

[unneeded] save-counted-string ?(
: save-counted-string ( ca1 len1 -- ca2 )
  dup 1+ allocate-string dup >r place r> ; ?)

  \ doc{
  \
  \ save-counted-string ( ca1 len1 -- ca2 )
  \
  \ Save the string _ca1 len1_ in the `stringer` as a counted
  \ string and return its new address _ca2_.
  \
  \ See also: `allocate-string`, 
  \
  \ }doc

( string/ char-in-string? char-position? ruler )

code string/ ( ca1 len1 len2 -- ca2 len2 )
  D9 c, C1 c, D1 c, E1 c, 19 c, A7 c, ED c, 42 c,
    \                             ; T  B
    \                             ; -- --
    \ exx           ; save IP     ; 04 01
    \ pop bc        ; len2        ; 10 01
    \ pop de        ; len1        ; 10 01
    \ pop hl        ; ca1         ; 10 01
    \ add hl,de                   ; 11 01
    \ and a         ; cy=0        ; 04 01
    \ sbc hl,bc     ; hl=ca2      ; 15 02
  E5 c, C5 c, D9 c, jpnext, end-code
    \ push hl                     ; 11 01
    \ push bc                     ; 11 01
    \ exx           ; restore IP  ; 04 01
    \ jp (ix)                     ; 08 02
    \                             ; -- --
    \                             ; 98 13 Total

  \ doc{
  \
  \ string/ ( ca1 len1 len2 -- ca2 len2 )
  \
  \ Return the _len2_ ending characters of string _ca1 len1_.
  \
  \ See also: `/string`.
  \
  \ }doc

[unneeded] char-in-string? ?( need -rot

: char-in-string? ( ca len c -- f )
  -rot bounds ?do   dup i c@ = if  drop true unloop exit  then
              loop  drop false ; ?)

  \ doc{
  \
  \ char-in-string? ( ca len c -- f )
  \
  \ Is char _c_ in string _ca len_?
  \
  \ See also: `char-position?`, `contains`, `compare`,
  \ `#chars`.
  \
  \ }doc

[unneeded] char-position? ?( need -rot

: char-position? ( ca len c -- +n true | false )
  -rot 0 ?do   2dup i + c@ = if  2drop i true unloop exit  then
         loop  2drop false ; ?)

  \ doc{
  \
  \ char-position? ( ca len c -- +n true | false )
  \
  \ If char _c_ is in string _ca len_, return its first
  \ position _+n_ and _true_; else return _false_.
  \
  \ See also: `char-in-string?`, `contains`, `compare`.
  \
  \ }doc

[unneeded] ruler ?(

: ruler ( c len -- ca len )
  dup allocate-string swap 2dup 2>r rot fill 2r> ; ?)

  \ doc{
  \
  \ ruler ( c len -- ca len )
  \
  \ Return a string _ca len_ of characters _c_.
  \
  \ See also: `chars>string`, `char>string`, `s+`.
  \
  \ }doc

( sconstant sconstants unescape )

[unneeded] sconstant ?(

: sconstant ( ca len "name" -- )
  here >r s, r> count 2constant ; ?)

  \ doc{
  \
  \ sconstant ( ca len "name" -- )
  \
  \ Create a string constant _name_ with value _ca len_.
  \
  \ See also: `sconstants`.
  \
  \ }doc

[unneeded] sconstants ?( need array> need sconstants-does>

: sconstants ( 0 ca[n]..ca[1] "name" -- n )
  create  0 begin  swap ?dup  while  , 1+  repeat
  does> ( n -- ca len ) ( n pfa ) array> @ count ; ?)

  \ doc{
  \
  \ sconstants ( 0 ca[n]..ca[1] "name" -- n )
  \
  \ Create a table of string constants _name_, using counted
  \ strings _ca[n]..ca[1]_, being _0_ a mark for the last
  \ string on the stack, and return the number _n_ of compiled
  \ strings.
  \
  \ When _name_ is executed, it converts the index on the stack
  \ (0.._n-1_) to the correspondent string _ca len_.
  \
  \ Usage example:

  \ ----
  \
  \ 0                \ end of strings
  \   here ," kvar"  \ string 4
  \   here ," tri"   \ string 3
  \   here ," du"    \ string 2
  \   here ," unu"   \ string 1
  \   here ," nul"   \ string 0
  \ sconstants digitname
  \   constant digitnames
  \
  \ cr .( There are ) digitnames . .( digit names:)
  \ 0 digitname cr type
  \ 1 digitname cr type
  \ 2 digitname cr type
  \ 3 digitname cr type cr
  \ ----

  \ See also: `sconstant`.
  \
  \ }doc

[unneeded] unescape ?(

: unescape ( ca1 len1 ca2 -- ca2 len2 )
  dup 2swap over + swap ?do
    i c@ '%' = if '%' over c! 1+ then
    i c@ over c! 1+
  loop  over - ; ?)

  \ Credit:
  \
  \ Implementation from the Forth-2012 documentation.

  \ doc{
  \
  \ unescape ( ca1 len1 ca2 -- ca2 len2 )
  \
  \ Replace each "%" character in the input string _ca1 len1_
  \ by two "%" characters. The output is represented by _ca2
  \ len2_.  The  buffer at ca2  shall be big enough  to hold
  \ the unescaped string.
  \
  \ If you pass a string through `unescape` and then
  \ `substitute`, you get the original string.
  \
  \ Origin: Forth-2012 (STRING EXT).
  \
  \ See also: `replaces`.
  \
  \ }doc

  \ vim: filetype=soloforth
