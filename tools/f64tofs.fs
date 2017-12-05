#! /usr/bin/env gforth

\ f64tofs.fs

\ This file is part of Solo Forth
\ http://programandala.net/en.program.solo_forth.html

\ Last modified: 201712051808
\ See change log at the end of the file

\ Author: Marcos Cruz (programandala.net), 2017.

\ =============================================================
\ Description

\ This program converts a binary file containing a ZX Spectrum
\ 64-cpl 336-byte font, as used by Solo Forth's `mode-64o` and
\ `mode-64s` drivers, into its equivalent Forth source, to be
\ edited and included into the Solo Forth's library as an
\ alternative to the binary file.

\ =============================================================
\ Usage example

\ f64tofs.fs path-to/my-font.f64 > path-to/my-font.fs

\ =============================================================

warnings off

variable character  bl character !

7 constant bytes/line

create byte 0 c,

: ?byte ( n -- ) 0= abort" Unexpected end of file" ;

: read-byte ( fid -- ) byte 1 rot read-file throw ?byte ;

: .byte ( b -- ) base @ swap hex s>d <# # # #> type ."  c, "
                 base ! ;

: convert-byte ( -- ) read-byte byte c@ .byte ;

: convert-bytes ( fid -- )
  bytes/line 0 ?do dup convert-byte loop drop ;

: ascii? ( c -- f ) bl 127 within ;

: printable ( c1 -- c2 ) dup ascii? 0= if drop '.' then ;

: comment ( -- )
  ." \ " character @ dup printable emit
                      1+ printable emit 2 character +! ;

: convert-line ( fid -- ) convert-bytes comment cr ;

: last-line? ( -- f ) character @ ascii? 0= ;

: (convert) ( fid -- )
  begin dup convert-line last-line? until drop ;

: open ( ca len -- fid ) r/o open-file throw ;

: close ( fid -- ) close-file throw ;

: convert ( ca len -- ) open dup (convert) close ;

1 arg convert bye

\ =============================================================
\ Change log

\ 2017-12-05: First version.
