  \ math.operators.4-cell.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Quadruple-cell operators.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( q2* )

  \ Credit:
  \
  \ Original code by Wil Baden, published on Forth Dimensions
  \ 18/5 p. 29 (1997-01).

need d2*

: q2* ( n . . . -- 2n . . . )
  d2* >r >r  dup 0< if    d2* r> 1+ r>
                    else  d2* r> r>  then ;

( q+ q- q0< q0= qu< qnegate qabs )

  \ Credit:
  \
  \ copyright 1990-2007  Frank Sergeant
  \ License:  http://pygmy.utoh.org/license.html

code q+ ( nq1 nq2 -- nq3 )
  \ XXX TODO -- port to Z80
end-code

code q- ( nq1 nq2 -- nq3 )
  \ XXX TODO -- port to Z80
end-code

: q0< ( nq -- f ) 0< push drop 2drop pop ;
: q0= ( nq -- f ) or or or 0= ;
: qu< ( uq uq -- f ) q- q0< ;
: qnegate ( nq -- nq' ) 0 0 0 0 4swap q- ;
: qabs ( nq -- uq ) dup 0< if  qnegate  then ;

( udm* dm* )

  \ Credit:
  \
  \ copyright 1990-2007  Frank Sergeant
  \ License:  http://pygmy.utoh.org/license.html

need qnegate

code udm* ( ud ud - quad )
  \ XXX TODO -- port to Z80
end-code

: dm* ( nd nd - nq )
  2>r dup 0< dup >r if  dnegate  then
  r> 2r> dup 0< dup >r if  dnegate  then
  rot >r udm* 2r> xor 0< if  qnegate  then ;

  \ ===========================================================
  \ Change log

  \ 2015-11-13: Add `q2*`.
  \
  \ 2016-12-30: Modify the layout of two words.

  \ vim: filetype=soloforth
