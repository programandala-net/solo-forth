  \ strings.escaped.graphics.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804142327
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to include graphics characters in escaped strings.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( first-esc-block-char esc-block-chars-wordlist )

get-current  forth-wordlist set-current

need parse-esc-char>chars need even? need nextname

variable first-esc-block-char  128 first-esc-block-char !

  \ doc{
  \
  \ first-esc-block-char ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ code of the first block graphic.  Its default value is 128,
  \ like in the ZX Spectrum charset.  This variable can be
  \ modified in order to make the escaped block characters
  \ produce a different range of codes.
  \
  \ See: `esc-block-chars-wordlist`.
  \
  \ }doc

wordlist dup constant esc-block-chars-wordlist
         dup >order set-current

  \ doc{
  \
  \ esc-block-chars-wordlist ( -- wid )
  \
  \ Identifier of the word list that contains the escaped block
  \ characters used by the BASin IDE and other ZX Spectrum
  \ tools:
  \

  \ |===
  \ | Escaped notation  | Default character code
  \
  \ | \<space><space>   | 128
  \ | \<space>'         | 129
  \ | \'<space>         | 130
  \ | \''               | 131
  \ | \<space>.         | 132
  \ | \<space>:         | 133
  \ | \'.               | 134
  \ | \':               | 135
  \ | \.<space>         | 136
  \ | \.'               | 137
  \ | \:<space>         | 138
  \ | \:'               | 139
  \ | \..               | 140
  \ | \.:               | 141
  \ | \:.               | 142
  \ | \::               | 143
  \ |===

  \ In order to make `s\", `.\"` and their common factor
  \ `parse-esc-string` recognize the escaped block characters,
  \ `esc-block-chars-wordlist` must be pushed to the escaped
  \ strings search order.  Example:

  \ ----
  \ need set-esc-order
  \ esc-standard-chars-wordlist
  \ esc-block-chars-wordlist 2 set-esc-order
  \
  \ s\" \::\:.\ '\. \nNew line:\.'\:'\'.\: ..." type
  \ ----

  \ The code of the first block character can be modified with
  \ the character variable `first-esc-block-char`.
  \
  \ See: `first-esc-block-char`, `set-esc-order`, `>esc-order`,
  \ `esc-standard-chars-wordlist`, `esc-udg-chars-wordlist`,
  \ `parse-esc-string`, `s\"`, `.\"`.
  \
  \ }doc

variable column  column off
  \ Current column of the block character being escaped.
  \ The block characters notation uses 2 colums, two characters.
  \ Only the first bit of this variable is checked.

: left-column? ( -- f ) column @ even?  1 column +! ;
  \ Is the left column of the block character notation being
  \ escaped? Update it for the next time.

: >parsed-block-char ( n1 n2 -- c 1 )
  + first-esc-block-char @ + 1 ;
  \ Convert the values _n1 n2_ of the first and second
  \ columns of the block character notation to the actual
  \ character _c_ and its count _1_.

-->

( esc-block-chars-wordlist )

s"  " nextname
:    ( "c" | n1 -- n1 | n2 1 )
  left-column? if    0 parse-esc-char>chars
               else  0 >parsed-block-char  then ;
  \ Parse a column " " of a escaped block character.
  \ Note: The name of this word is a space.

: ' ( "c" | n1 -- n1 | n2 1 )
  left-column? if    2 parse-esc-char>chars
               else  1 >parsed-block-char then ;
  \ Parse a column "'" of a escaped block character.

: . ( "c" | n1 -- n1 | n2 1 )
  left-column? if    8 parse-esc-char>chars
               else  4 >parsed-block-char  then ;
  \ Parse a column "," of a escaped block character.

: : ( "c" | n1 -- n1 | n2 1 )
  left-column? if    10 parse-esc-char>chars
               else   5 >parsed-block-char  then ;
  \ Parse a column ":" of a escaped block character.

set-current previous

( esc-udg-chars-wordlist )

get-current

wordlist dup constant esc-udg-chars-wordlist set-current

  \ doc{
  \
  \ esc-udg-chars-wordlist ( -- wid )
  \
  \ Identifier of the word list that contains the words whose
  \ names are the UDG characters ('A'..'U'), in upper case,
  \ that must be escaped after a backslash in strings parsed by
  \ `s\"`, `.\"` and other words.
  \
  \ The execution of the words defined in the word list
  \ identified by ``esc-udg-chars-wordlist`` returns the
  \ correspondent UDG character (144..164) and a 1.
  \
  \ In order to make `s\"`, `.\"` and their common factor
  \ `parse-esc-string` recognize the escaped UDG characters,
  \ `esc-udg-chars-wordlist` must be pushed on the escaped
  \ strings search order.  Example:

  \ ----
  \ need set-esc-order
  \ esc-standard-chars-wordlist
  \ esc-udg-chars-wordlist 2 set-esc-order
  \
  \ s\" \A\B\C\D\nNew line:\A\B\C\D..." type
  \ ----

  \ See: `set-esc-order`, `>esc-order`,
  \ `esc-standard-chars-wordlist`, `esc-block-chars-wordlist`.
  \
  \ }doc

case-sensitive @ case-sensitive on

144 1 2constant A  145 1 2constant B  146 1 2constant C
147 1 2constant D  148 1 2constant E  149 1 2constant F
150 1 2constant G  151 1 2constant H  152 1 2constant I
153 1 2constant J  154 1 2constant K  155 1 2constant L
156 1 2constant M  157 1 2constant N  158 1 2constant O
159 1 2constant P  160 1 2constant Q  161 1 2constant R
162 1 2constant S  163 1 2constant T  164 1 2constant U

case-sensitive !

set-current

  \ ===========================================================
  \ Change log

  \ 2016-12-21: Start.
  \
  \ 2016-12-22: Adapt to the changes in the
  \ <strings.escaped.fsb> module. Improve. Document. Fix
  \ `left-column?`: use `even?` instead of `odd?`.
  \
  \ 2016-12-23: Remove debugging code. Use case-sensitive mode
  \ to define the UDG escaped chars.
  \
  \ 2016-12-27: Fix escaped UDG "\C".
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-04-14. Fix markup in documentation.

  \ vim: filetype=soloforth
