  \ game.pong.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201703132341
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The Pong game.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ Credit

  \ Based on code included in IsForth (version 1.23v):
  \
  \ pong.f
  \ Written june 2002 by Robert Oestling
  \ <robost at telia dot com>
  \ Tested with IsForth, http://isforth.clss.net/
  \
  \ Ported to IsForth by Mark Manning, 2012.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- bounce effect
  \ XXX TODO -- sound
  \ XXX TODO -- turnkey to tape
  \ XXX TODO -- change the rotation of the ball after bounce
  \ XXX TODO -- random bounce angle
  \ XXX TODO slow the rackets

( pong )

only forth definitions

need columns need rows need udg[ need rnd need ??
3 constant /kk need pressed?

need cvariable need 2/ need gxy>scra
need g-emit-udg need ctoggle need g-at-xy

need black need white need set-ink need brighty need papery

wordlist dup constant pong-wordlist dup >order set-current

8 cconstant ball-delay0
  \ Counter: Times the ball is not moved in the main loop.

variable ball-delay

4 cconstant racket-size

white papery brighty cconstant racket-color

22528 constant top-line-attr
  \ address of the top left screen attribute
23264 constant bottom-line-attr
  \ address of the bottom left screen attribute

code sync ( -- ) 78 c,  jpnext, end-code  -->
  \ Z80 halt

( pong )

  \ Key constant are defined with double constants this way:
  \ high part = bitmask
  \ low part = port of the keyboard row

$01 $F7FE 2constant left1-key   '1' cconstant left1-char
$02 $F7FE 2constant right1-key  '2' cconstant right1-char
  \ Player 1 keys.

$10 $EFFE 2constant left2-key   '6' cconstant left2-char
$08 $EFFE 2constant right2-key  '7' cconstant right2-char
  \ Player 2 keys.

variable x  variable y
  \ Coordinates of the ball.

cvariable direction
  \ Direction of ball.
  \ Bit 0: 1 = down, 0 = up.
  \ Bit 1: 1 = right, 0 = left.

variable points1  variable points2
  \ Player points.

variable racket1-x  columns racket-size - 2/  racket1-x !
  \ Top racket x coordinate.

variable racket2-x  racket1-x @ racket2-x !
  \ Bottom racket x coordinate.

0 cconstant racket1-y
  \ Top racket y coordinate.

rows 1- cconstant racket2-y
  \ Bottom racket y coordinate.

-->

( pong )

0 udg[

00111100 | 00111100 | 00111100 | 00111100 | 00111100 ||
01111110 | 01111110 | 01111110 | 01111110 | 01111110 ||
11111011 | 11111111 | 11111111 | 11111111 | 11111111 ||
11111101 | 11111101 | 11111111 | 11111111 | 11111111 ||
11111111 | 11111101 | 11111101 | 11111111 | 11111111 ||
11111111 | 11111111 | 11111011 | 11110011 | 11100111 ||
01111110 | 01111110 | 01111110 | 01111110 | 01111110 ||
00111100 | 00111100 | 00111100 | 00111100 | 00111100 ||]

-->

( pong )

5 udg[

00111100 | 00111100 | 00111100 | 00111100 | 00111100 ||
01111110 | 01111110 | 01111110 | 01111110 | 01011110 ||
11111111 | 11111111 | 11111111 | 10111111 | 10111111 ||
11111111 | 11111111 | 10111111 | 10111111 | 11111111 ||
11111111 | 10111111 | 10111111 | 11111111 | 11111111 ||
11001111 | 11011111 | 11111111 | 11111111 | 11111111 ||
01111110 | 01111110 | 01111110 | 01111110 | 01111110 ||
00111100 | 00111100 | 00111100 | 00111100 | 00111100 ||]

-->

( pong )

10 udg[

00111100 | 00111100 | 00111100 ||
01001110 | 01100110 | 01110110 ||
11111111 | 11111111 | 11111011 ||
11111111 | 11111111 | 11111111 ||
11111111 | 11111111 | 11111111 ||
11111111 | 11111111 | 11111111 ||
01111110 | 01111110 | 01111110 ||
00111100 | 00111100 | 00111100 ||]

-->

( pong )

 0 constant first-ball-frame
12 constant last-ball-frame

variable ball-frame  first-ball-frame ball-frame !
  \ UDG number (0..127) of the current frame of the ball
  \ graphic.

: ball ( -- n ) ball-frame @ ;
  \ Return the UDG number _n_ (0..127) of the current frame
  \ of the ball graphic.

: last-ball-frame? ( -- f ) ball last-ball-frame = ;
  \ Is the current frame of the ball graphic the last one?

: next-ball-frame ( -- )
  last-ball-frame? if     first-ball-frame ball-frame !
                   else   1 ball-frame +!  then ;
  \ Update the current frame of the ball graphic.

: ball-xy ( -- x y ) x @ y @ ;

: at-ball ( -- ) ball-xy g-at-xy ;

: show-ball ( -- ) at-ball ball sync g-emit-udg ;

: erase-ball ( -- ) at-ball ball sync g-emit-udg ;

: restore-screen ( -- ) default-colors page ;

: init-screen ( -- ) restore-screen white set-ink ;  -->

( pong )

: (border) ( a -- )
  columns [ white papery ] literal sync fill ;

: top-border ( -- ) top-line-attr (border) ;

: bottom-border ( -- ) bottom-line-attr (border) ;

: show-racket ( a -- ) racket-size racket-color sync fill ;

: show-racket1 ( -- )
  top-line-attr dup (border) racket1-x @ + show-racket ;

: show-racket2 ( -- )
  bottom-line-attr dup (border) racket2-x @ + show-racket ;

: show-rackets ( -- ) show-racket1 show-racket2 ;

: racket-initial-x ( -- n )
  columns 2/ [ racket-size 2/ ] literal - ;

: reset-rackets ( -- )
  racket-initial-x dup racket1-x ! racket2-x ! ;

: erase-racket ( a1 a2 -- ) @ + racket-size erase ;  -->

  \ Erase a racket.
  \ a1 = address of the first screen attribute on the row
  \ a2 = variable that holds the racket x coordinate

( pong )

: erase-racket1 ( -- )
  top-line-attr racket1-x erase-racket ;
  \ Erase racket of player 1.

: erase-racket2 ( -- )
  bottom-line-attr racket2-x erase-racket ;
  \ Erase racket of player 2.

: (print-points) ( n y -- )
  0 swap at-xy s>d <# # # # #>
  black set-ink sync type white set-ink ;
  \ Print the points of a player.

: print-points1 ( -- )
  points1 @ racket1-y (print-points) show-racket1 ;
  \ Print the points of player 1.

: print-points2 ( -- )
  points2 @ racket2-y (print-points) show-racket2 ;
  \ Print the points of player 2.

: print-points ( -- ) print-points1 print-points2 ;
  \ Print the points of both players.

: change-x ( -- ) %10 direction ctoggle ;
  \ Change the x direction of the ball.

: change-y ( -- ) %01 direction ctoggle ;  -->
  \ Change the y direction of the ball.

  \ : faster ( -- ) exit ;  \ XXX OLD
  \   speed @ 40 > if  speed @ dup 20 / - speed !   then ;
  \ If the delay is more than 40 ms, reduce it with 5%.

( pong )

: ball-moving-right? ( -- 0f ) direction c@ %10 and ;
: ball-moving-down? ( -- 0f ) direction c@ %01 and ;
  \ : ball-at-right? ( -- f ) x @ columns 1- = ;
: ball-at-right? ( -- f ) x @ 247 > ;
: ball-at-left? ( -- f ) x @ 1 < ;
  \ : ball-at-bottom? ( -- f ) y @ rows 2- = ;
: ball-at-bottom? ( -- f ) y @ 16 < ;
  \ : ball-at-top? ( -- f ) y @ 1 = ;
: ball-at-top? ( -- f ) y @ 182 > ;

: move-ball-x ( -- )
  ball-moving-right?
  if
    \ 0 2 at-xy ." right" \ XXX INFORMER
    ball-at-right?  if  change-x  then  1
  else
    \ 0 2 at-xy ." left " \ XXX INFORMER
    ball-at-left?   if  change-x  then  -1
  then  x +! ;

: reset-ball ( -- )
  128 x !  95 y !  rnd %11 and direction c! ;  -->
  \ Reset the ball position and direction.

( pong )

: ready ( -- ) reset-rackets reset-ball ;

: score-player1 ( -- ) 1 points1 +! print-points1 ;
  \ Increase player 1's points by one.

: score-player2 ( -- ) 1 points2 +! print-points2 ;
  \ Increase player 2's points by one.

[defined] 8* ?\ : 8* ( n1 -- n2 ) 2* 2* 2* ;

: hit-racket1? ( -- f )
  x @ racket1-x @ 8* 1- >
  x @ racket1-x @ 8* racket-size 8* + <  and ;
  \ Is racket1 hit by the ball?
  \ Is racket1-x <= x < racket1-x + racket-size?

: hit-racket2? ( -- f )
  x @ racket2-x @ 8* 1- >
  x @ racket2-x @ 8* racket-size 8* + <  and ;
  \ Is racket2 hit by the ball?
  \ Is racket2-x <= x < racket2-x + racket-size?

  \ : hit-racket? ( a -- f )
  \   \ XXX NEW -- alternative
  \   \ XXX TODO try
  \   \ a = address that holds the x coordinate of a racket
  \   @ 8* x @ swap ( ball-x pad-x )
  \   2dup 1- > >r
  \   racket-size 8* + <  r> and ;

: possible-top-hit ( -- )
  hit-racket2? if change-y  else  score-player1 ready  then ;

: move-ball-down ( -- )
  ball-at-bottom? if  possible-top-hit  else  -1 y +!  then ;

-->

( pong )

: possible-bottom-hit ( -- )
  hit-racket1? if  change-y  else  score-player2 ready  then ;

: move-ball-up ( -- )
  ball-at-top? if  possible-bottom-hit  else  1 y +!  then ;

: move-ball-y ( -- )
  ball-moving-down?
  if  move-ball-down  else  move-ball-up  then ;

8 cconstant racket-delay0
  \ Counter: Times the rackets are not moved in the main loop.
variable racket1-delay
variable racket2-delay

: ?move-ball ( -- )
  -1 ball-delay +!  ball-delay @ if  unnest  exit  then
  ball-delay0 ball-delay ! ;
  \ Update the delay of the ball.
  \ If the ball must not be moved, exit from the calling word.

-->

( pong )

: move-ball ( -- )
  ?move-ball erase-ball move-ball-x move-ball-y
             next-ball-frame show-ball ;
  \ Move the ball.

: frame ( -- ) white border  top-border bottom-border ;
  \ Draw the frame of the arena.

: arena-line ( -- )
  [ 0 96 gxy>scra nip ] literal columns %10101010 fill
  [ 0 95 gxy>scra nip ] literal columns %01010101 fill ;
  \ Draw the line of the arena.

: arena ( -- )
  cls  frame arena-line show-rackets print-points show-ball ;
  \ Draw the arena.

: ?move-racket1 ( -- )
  -1 racket1-delay +!  racket1-delay @ if  unnest exit  then
  racket-delay0 racket1-delay ! ;

: (move-racket1) ( 1|-1 -- ) racket1-x +!  show-racket1 ;

-->

( pong )

: move-racket1-left ( -- )
  ?move-racket1
  racket1-x @ 0= ?exit
  -1 (move-racket1) ;

: move-racket1-right ( -- )
  ?move-racket1
  racket1-x @ racket-size + columns = ?exit
  1 (move-racket1) ;

: ?move-racket2 ( -- )
  -1 racket2-delay +!  racket2-delay @ if  unnest exit  then
  racket-delay0 racket2-delay ! ;

: (move-racket2) ( 1|-1 -- ) racket2-x +!  show-racket2 ;

-->

( pong )

: move-racket2-left ( -- )
  ?move-racket2
  racket2-x @ 0= ?exit
  -1 (move-racket2) ;

: move-racket2-right ( -- )
  ?move-racket2
  racket2-x @ racket-size + columns = ?exit
  1 (move-racket2) ;

: reset-points ( -- ) points1 off  points2 off ;

: init-game ( -- )
  init-screen reset-points
  racket-delay0  dup racket1-delay !  racket2-delay !
  ball-delay0 ball-delay ! ready ;

-->

( pong )

: quit-game ( -- )
  restore-screen  ." Player 1 score: " points1 ? cr
                  ." Player 2 score: " points2 ?  quit ;

: keypress ( key -- )
  left1-key     pressed? ?? move-racket1-left
  right1-key    pressed? ?? move-racket1-right
  left2-key     pressed? ?? move-racket2-left
  right2-key    pressed? ?? move-racket2-right
              break-key? ?? quit-game ;

: show-player-key ( c ca len -- )
  space rot emit ."  = " type cr ;

: show-player-keys ( c1 c2 -- )
  s" left" show-player-key  s" right" show-player-key ;  -->

( pong )

: show-game-keys ( -- )
  ." Player 1:" cr right1-char left1-char show-player-keys
  ." Player 2:" cr right2-char left2-char show-player-keys
  ." Break (Shift+Space) = quit" ;

: show-credits ( -- )

  \  <------------------------------>
  ." Pong" cr cr
  ." Original code by:" cr
  ."   Robert Oestling, 2002" cr
  ." Ported to IsForth by:" cr
  ."   Mark Manning, 2012" cr
  ." Rewritten for Solo Forth by:" cr
  ."   Marcos Cruz" cr
  ."   (programandala.net), 2015," cr
  ."   2016." cr ;
  \  <------------------------------>

-->

( pong )

: press-any-key ( -- )
  \  <------------------------------>
  ." Press any key to start the game." key drop ;

: welcome ( -- )
  page show-credits cr show-game-keys cr cr press-any-key ;

: pong ( -- )
  init-game welcome page arena
  begin  move-ball
    \ begin key? until key drop  \ XXX TMP -- for debugging
  keypress  again ;

  \ XXX TMP -- for debugging

: pong-test ( -- )
  begin
    last-ball-frame 1+ 128 + first-ball-frame 128 + ?do
      home i emit  key drop
    loop
  break-key? until ;

cr .( Type PONG to run)

  \ ===========================================================
  \ Change log

  \ 2015-11-09: First working version.
  \
  \ 2015-11-11: Improve with delay counters and Z80 halts.
  \
  \ 2016-04-23: Improve file header. Add frames to the ball
  \ graphic. Adapt to new version of `g-emit-0udg`.
  \
  \ 2016-04-24: Compact the UDG definitions with `0udg[`.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-11-23: Rename `c!toggle-bits` to `ctoggle`, after the
  \ changes in the system.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2016-12-30: Compact the source, saving some blocks.
  \
  \ 2017-01-31: Update the attribute and color words.
  \
  \ 2017-02-04: Adapt to 0-index-only UDG, after the changes in
  \ the kernel and the library.
  \
  \ 2017-02-05: Fix loading: a comment had removed a `-->`.
  \ Add `pong-wordlist` for the words of the game. Show help
  \ message at the end of the loading.
  \
  \ 2017-03-13: Update name: `pixel-addr` to `gxy>scra`.

  \ vim: filetype=soloforth
