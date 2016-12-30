#! /usr/bin/env gforth

\ patch_the_loader.fs

\ This file is part of Solo Forth
\ http://programandala.net/en.program.solo_forth.html

\ Last modified 201612302012

\ ==============================================================
\ Description

\ This program patches the BASIC loader of Solo Forth with the
\ proper code address, extracted from the Z80 symbols file
\ created during the compilation of the kernel.

\ This program is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth/).

\ ==============================================================
\ Author

\ Marcos Cruz (programandala.net), 2016.

\ ==============================================================
\ License

\ You may do whatever you want with this work, so long as you
\ retain every copyright, credit and authorship notice, and this
\ license.  There is no warranty.

\ ==============================================================
\ History

\ 2016-04-13: First version, with hardcoded filenames.
\
\ 2016-04-16: Improved: the filenames are get from arguments.
\
\ 2016-12-30: Improve the description.

\ ==============================================================
\ Requirements

only forth definitions

\ --------------------------------------------------------------
\ From Forth Foundation Library

require ffl/str.fs

\ --------------------------------------------------------------
\ From Galope (http://programandala.net/en.program.galope.html)

: unslurp-file  ( ca1 len1 ca2 len2 -- )
  w/o create-file throw >r
  r@ write-file throw
  r> close-file throw  ;
  \ Write string _ca1 len1_ to file _ca2 len2_.

str-create tmp-str

: replaced ( ca1 len1 ca2 len2 ca3 len3 -- ca1' len1' )
  2rot tmp-str str-set  tmp-str str-replace  tmp-str str-get  ;
  \ Replace all ocurrences of _ca3 len3_ with _ca2 len2_ in
  \ _ca1 len1_.

\ ==============================================================
\ Arguments

1 arg 2constant symbols-file
2 arg 2constant raw-loader-file
3 arg 2constant patched-loader-file

\ ==============================================================
\ Main

\ --------------------------------------------------------------
\ Number conversion

: >value  ( ca len -- d )
  1- 0. 2swap base @ >r hex >number 2drop  r> base !  ;
  \ Get the symbol value from a string _ca len_, which contains an
  \ hex number with a trailing "H".

: d>str  ( d -- ca len )  <# #s #>  ;

: >decimal  ( ca1 len1 -- ca2 len2 )  >value d>str  ;
  \ Convert the hex number contained in string
  \ _ca1 len1_, which has a trailing "H",
  \ to decimal representation in string _ca2 len2_.

: patch-symbol  ( ca1 len1 ca2 len2 ca3 len3 -- ca1' len1' )
  >decimal 2swap replaced  ;
  \ Replace symbol _ca2 len2_ in string _ca1 len1_ with the
  \ decimal value of hex number contained in string _ca2 len2_.

: symbol:  ( "name" -- )
  create  latest name>string s,
  does>  ( ca len "name1" "name2" -- ca' len' )
  ( pfa ) count parse-name 2drop
          parse-name patch-symbol  ;
  \ Create a symbol "name" which will parse its own value
  \ "name2" from the current source (the symbols file), and
  \ patch the loader _ca len_ with it, returning an updated
  \ version _ca' len'_.

\ Create the only words that will be recognized and executed
\ while interpreting the symbols file:

wordlist constant symbols-wordlist

symbols-wordlist set-current

  symbol: origin      ( ca len "name1" "name2" -- ca' len' )
  symbol: ramtop      ( ca len "name1" "name2" -- ca' len' )
  symbol: cold_entry  ( ca len "name1" "name2" -- ca' len' )
  symbol: warm_entry  ( ca len "name1" "name2" -- ca' len' )

forth-wordlist set-current

\ --------------------------------------------------------------
\ Parser

: (parse-symbols)  ( ca len -- )
  begin   parse-name ?dup
  while   find-name ?dup if  name>int execute  then
  repeat  drop  ;
  \ Parse the current source in order to patch the loader
  \ _ca len_.

: parse-symbols  ( ca1 len1 ca2 len2 -- )
  symbols-wordlist 1 set-order
  ['] (parse-symbols) execute-parsing
  only forth  ;
  \ Parse the symbols _ca2 len2_ with the symbols word list, in
  \ order to patch the loader _ca1 len1_.

\ --------------------------------------------------------------
\ Boot

: read-loader   ( -- ca len )  raw-loader-file slurp-file  ;
: write-loader  ( ca len -- )  patched-loader-file unslurp-file  ;
: read-symbols  ( -- ca len )  symbols-file slurp-file  ;

: run  ( -- ) read-loader read-symbols parse-symbols write-loader  ;

run bye

\ vim: ts=2:sts=2:et:tw=64
