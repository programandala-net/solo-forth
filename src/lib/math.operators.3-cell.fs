  \ math.operators.3-cell.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803072222
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Triple-cell operators.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( tum* tum/ t+ t- )

  \ XXX TODO -- test

unneeding tum*
?\ : tum* ( d n -- t ) 2>r  r@ um*  0 2r>  um* d+ ;
  \ Triple unsigned mixed multiply.

unneeding t+ ?(

: +carry ( n1 n2 -- n1+n2 carry ) 0 tuck d+ ;

: t+ ( t1 t2 -- t3 )
  >r rot >r  >r swap >r +carry  0 r> r> +carry d+ r> r> + + ;

?)
  \ Triple add.

unneeding tum/ ?(
: tum/ ( t n -- d ) dup >r um/mod r> swap >r um/mod nip r> ;
?)
  \ Triple unsigned mixed division.

unneeding t- ?( need d-

: -borrow ( n1 n2 -- n1-n2 borrow ) 0 tuck d- ;

: t- ( t1 t2 -- t3 ) >r rot >r  >r swap >r -borrow
                     s>d r> r> -borrow d+ r> r> - + ; ?)
  \ Triple substract.

  \ Credit:
  \
  \ All words by Wil Baden, published on Forth Dimensions
  \ (volume 19, number 6, page 34, 1998-04).

( mt* ut/ ut* tnegate )

unneeding ut*

?\ : ut* ( ud u -- t ) swap >r dup >r  um* 0 r> r> um* d+ ;

  \ Credit:
  \ Robert Smith (from COLDFORTH Version 0.8, GPL)
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/double.f

  \ XXX TODO -- test

  \ doc{
  \
  \ ut*   ( ud u -- t ) "u-t-star"
  \
  \ _t_ is the signed product of _ud_ times _u_.
  \
  \ }doc

unneeding mt* ?( need ut* need tnegate

: mt*   ( d n -- t ) dup 0<
  if    abs over 0< if  >r dabs r> ut*  else  ut* tnegate  then
  else  over 0< if  >r dabs r> ut* tnegate  else  ut*   then
  then ; ?)

  \ Credit:
  \ Robert Smith (from COLDFORTH Version 0.8, GPL)
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/double.f

  \ XXX TODO -- test

  \ doc{
  \
  \ mt*   ( d n -- t ) "m-t-star"
  \
  \ _t_ is the signed product of _d_ times _n_.
  \
  \ }doc

unneeding ut/

?\ : ut/ ( ut n -- d ) dup >r um/mod -rot r> um/mod nip swap ;

  \ Credit:
  \ Robert Smith (from COLDFORTH Version 0.8, GPL)
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/double.f

  \ XXX TODO -- test

  \ doc{
  \
  \ ut/   ( ut n -- d ) "u-t-slash"
  \
  \ Divide a triple unsigned number _ut_ by a single number _n_
  \ giving the double number result _d_.
  \
  \ }doc

unneeding tnegate ?(

: tnegate ( t1 -- t2 ) invert >r invert >r
                       invert 0 -1 -1 d+ s>d r> 0 d+ r> + ; ?)

  \ Credit:
  \
  \ Code by Robert Smith (from COLDFORTH Version 0.8, GPL)
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/double.f

  \ XXX TODO -- test

  \ doc{
  \
  \ tnegate ( t1 -- t2 ) "t-negate"
  \
  \ _t2_ is the negation of _t1_.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-30: Compact the code, saving two blocks. Make
  \ `tum*`, `tum/`, `t+` and `t-` accessible to `need`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.

  \ vim: filetype=soloforth
