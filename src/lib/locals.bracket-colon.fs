  \ locals.bracket-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804012117
  \ See change log at the end of the file

  \ XXX UNDER DEVELOPMENT

  \ ===========================================================
  \ Description

  \ An implementation of Forth-2012 `{:` locals syntax, adapted
  \ from the Forth-2012 documentation.

  \ ===========================================================
  \ Author

  \ Unknown.
  \
  \ Marcos Cruz (programandala.net) adapted the code to Solo
  \ Forth, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( {: )

need str<> need (local)

12345 constant undefined-value

: match-or-end? ( ca1 len1 ca2 len2 -- f )
  2 pick 0= >r str= r> or ;

: scan-args
  \ ( 0 ca1 len1 -- ca1 len1 ... ca@n len#n n ca#n+1 len#n+1 )
  begin 2dup s" |"  match-or-end? 0=
  while 2dup s" --" match-or-end? 0=
  while 2dup s" :}" match-or-end? 0=
  while rot 1+ parse-name
  again then then then ;

-->

( {: )

: scan-locals
  \ ( n ca1 len1 -- ca1 len1 .. ca#n len#n n ca#n+1 len#n+1 )
  2dup s" |" str<> ?exit
  2drop parse-name
  begin 2dup s" --" match-or-end? 0=
  while 2dup s" :}" match-or-end? 0=
  while rot 1+ parse-name postpone undefined-value
  again then then ;

: scan-end ( ca1 len1 -- ca2 len2 )
  begin  2dup s" :}" match-or-end? 0=
  while  2drop parse-name
  repeat ;

: define-locals ( ca1 len1 .. ca#n len#n n -- )
  0 ?do (local) loop 0 0 (local) ;

: {: ( -- ) 0 parse-name scan-args scan-locals scan-end
            2drop define-locals ; immediate

  \ ===========================================================
  \ Change log

  \ 2018-04-01: Start adaption of the original code: Make it
  \ fit in blocks. Update source style; convert to lowercase.

  \ vim: filetype=soloforth
