#! /usr/bin/env gforth

\ nm2absolute.fs

\ This file is part of Solo Forth
\ http://programandala.net/en.program.solo_forth.html

\ This program is written in Forth with Gforth, as part of the
\ development tools of Solo Forth.
\
\ This program defines the words required to interpret a symbols list
\ created by GNU binutils' nm as a Forth source, creating a new list
\ with absolute values. See how this program in invoked in <Makefile>.

\ By Marcos Cruz (programandala.net)

\ 2015-08-17

vocabulary nm  also nm definitions

: address.  ( n -- )  s>d <<# # # # # #> #>> type space  ;
  \ Print a number _n_ as a 32-bit 4-digit address.
  \ The radix is supposed to be hex.

: section:  ( n1 "name1" -- )
  create ,  does>  ( n2 "name2" -- ) ( n pfa "name" )
    @ + address. parse-name type cr ;
  \ Create a word _name1_ for a section that starts at address _n1_.
  \ The word _name1_ will parse a symbol _name2_ whose relative
  \ value is _n2_ and will print them after converting _n2_ to
  \ an absolute address.
 
0x5E00 section: t  \ text section
0xC000 section: d  \ data section
     0 section: a  \ unused section for absolute values
     0 section: b  \ unused bss section
hex \ the relative values will be hex

