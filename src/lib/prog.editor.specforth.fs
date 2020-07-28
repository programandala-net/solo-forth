  \ prog.editor.specforth.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ This is the editor included with Specforth (also known as
  \ Artic Forth), a fig-Forth for ZX Spectrum.  Its original
  \ name is "Specforth Editor V1.1".
  \
  \ It has been adapted to Solo Forth.
  \
  \ Word descriptions and stack comments have been added after
  \ the Specforth manual, the Abersoft Forth manual and Dr.
  \ C.H. Ting's book _Systems Guide to fig-Forth_. The word
  \ `copy` has been adapted from Abersoft Forth. The word
  \ `text` has been rewritten.

  \ ===========================================================
  \ Authors

  \ Copyright (C) 1983 by Artic Computing Ltd.
  \ Written by Chris A. Thornton, 1983.

  \ Adapted to Solo Forth by Marcos Cruz (programandala.net),
  \ 2015, 2016, 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( specforth-editor )

only forth definitions

need list need update need flush need parse-all need vocabulary
need +loop need editor need r# need top

vocabulary specforth-editor ' specforth-editor is editor

  \ doc{
  \
  \ specforth-editor ( -- )
  \
  \ A `vocabulary` containing the Specforth block editor. When
  \ ``specforth-editor`` is loaded, it becomes the action of
  \ `editor`.

  \ [cols="30,70"]
  \ .Specforth block editor commands
  \ |===
  \ | Word | Description

  \ | `<<src-lib-prog-editor-specforth-fs, b>>`
  \ ``( -- )``
  \ | Used after `<<src-lib-prog-editor-specforth-fs, f>>` to
  \ backup the cursor by the length of the most recent text.

  \ | `<<src-lib-prog-editor-specforth-fs, c>>`
  \ ``( "ccc<eol>" -- )``
  \ | Copy in _ccc_ to the cursor line at the cursor position.

  \ | `clear`
  \ ``( n -- )``
  \ | Clear block _n_ with blanks and select for editing.

  \ | `copy`
  \ ``( n1 n2 -- )``
  \ | Copy block _n1_ to block _n2_.

  \ | `<<src-lib-prog-editor-specforth-fs, d>>`
  \ ``( n -- )``
  \ | Delete line _n_ but hold it in `pad`. Line 15 becomes
  \ free as all statements move up one line.

  \ | `delete`
  \ ``( n -- )``
  \ | Delete _n_ characters prior to the cursor.

  \ | `<<src-lib-prog-editor-specforth-fs, e>>`
  \ ``( n -- )``
  \ | Erase line _n_ with blanks.

  \ | `<<src-lib-prog-editor-specforth-fs, find>>`,
  \ ``( -- )``
  \ | Search for a match to the string at `pad`, from the
  \ cursor position until the end of block.  If no match found
  \ issue an error message and reposition the cursor at the top
  \ of the block.

  \ | `<<src-lib-prog-editor-specforth-fs, h>>`
  \ ``( n -- )``
  \ | Hold line _n_ at `pad` (used by system more often than by
  \ user).

  \ | `<<src-lib-prog-editor-specforth-fs, i>>`
  \ ``( n -- )``
  \ | Insert text from `pad` at line _n_, moving the old line
  \ _n_ down. Line 15 is lost.

  \ | `<<src-lib-prog-editor-specforth-fs, l>>`
  \ ``( -- )``
  \ | List the current block.

  \ | `<<src-lib-prog-editor-specforth-fs, m>>`
  \ ``( n -- )``
  \ | Move the cursor by _n_ characters. The position of the
  \ cursor on its line is shown by a "_" (underline).

  \ | `<<src-lib-prog-editor-specforth-fs, n>>`
  \ ``( -- )``
  \ | Find the next occurrence of the string found by an
  \ `<<src-lib-prog-editor-specforth-fs, f>>` command.

  \ | `<<src-lib-prog-editor-specforth-fs, p>>`
  \ ``( n "ccc<eol>" -- )``
  \ | Put _ccc_ on line _n_.

  \ | `<<src-lib-prog-editor-specforth-fs, r>>`
  \ ``( n -- )``
  \ | Replace line _n_ with the text in `pad`.

  \ | `<<src-lib-prog-editor-specforth-fs, s>>`
  \ ``( n -- )``
  \ | Spread at line _n_. Line _n_ and following lines are are
  \ moved down one line. Line _n_ becomes blank. Line 15 is
  \ lost.

  \ | `<<src-lib-prog-editor-specforth-fs, t>>`
  \ ``( n -- )``
  \ | Type line _n_ and save in `pad`.

  \ | `till`
  \ ``( "ccc<eol>" -- )``
  \ | Delete on the cursor line from the cursor till the end of
  \ string _ccc_.

  \ | `<<src-lib-prog-editor-specforth-fs, x>>`
  \ ``( "ccc<eol>" -- )``
  \ | Find and delete the next occurrence of the string _ccc_.

  \ |===

  \ See also: `gforth-editor`.
  \
  \ }doc

also editor definitions

: text ( "ccc<eol>" -- ) pad c/l 1+ blank parse-all pad place ;

  \ doc{
  \
  \ text ( "ccc<eol>" -- )
  \
  \ Part of `specforth-editor`:
  \ Parse the text string until end of line and store it into
  \ `pad` as a counted string, blank-filling the remainder of
  \ `pad` to `c/l` characters.
  \
  \ See also: `parse-all`.
  \
  \ }doc

: line ( n -- a )
  dup $FFF0 and #-266 ?throw scr @ line>string drop ;

  \ doc{
  \
  \ line ( n -- a )
  \
  \ Part of `specforth-editor`:
  \ Leave address _a_ of the beginning of line _n_ in the
  \ current block buffer. The block number is in `scr`.
  \ Read the disk block from disk if it is not already in the
  \ disk buffer.
  \
  \ See also: `line>string`.
  \
  \ }doc

: #locate ( -- n1 n2 ) r# @ c/l /mod ;

  \ doc{
  \
  \ #locate ( -- n1 n2 ) "number-sign-locate"
  \
  \ Part of `specforth-editor`:
  \ From the cursor pointer `r#` compute the line number _n2_
  \ and the character offset _n1_ in line number _n2_.
  \
  \ See also: `#lead`, `c/l`.
  \
  \ }doc

: #lead ( -- a n ) #locate line swap ;

  \ doc{
  \
  \ #lead ( -- a n ) "number-sign-lead"
  \
  \ Part of `specforth-editor`:
  \ From the cursor pointer `r#` compute the line address _a_
  \ in the block buffer and the offset from _a_ to the cursor
  \ location _n_.
  \
  \ See also: `#locate`, `#lag`.
  \
  \ }doc

: #lag ( -- ca n ) #lead dup >r + c/l r> - ;

  \ doc{
  \
  \ #lag ( -- ca n ) "number-sign-lag"
  \
  \ Part of `specforth-editor`:
  \ Return cursor address _ca_ and count _n_ after cursor till
  \ end of line.
  \
  \ See also: `#lead`.
  \
  \ }doc

: -move ( ca n -- ) line c/l cmove update ;

  \ doc{
  \
  \ -move ( ca n -- ) "minus-move"
  \
  \ Part of `specforth-editor`:
  \ Move a line of text from _ca_ to line _n_ of current block.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, m>>`, `c/l`, `cmove`, `update`.
  \
  \ }doc

: e ( n -- ) line c/l blank update ;

  \ doc{
  \
  \ e ( n -- )
  \
  \ A command of `specforth-editor`: Erase line _n_ with
  \ blanks.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `c/l`, `blank`,
  \ `update`.
  \
  \ }doc

: s ( n -- ) dup 1 - $0E ?do i line i 1+ -move -1 +loop e ;

  \ doc{
  \
  \ s ( n -- )
  \
  \ A command of `specforth-editor`: Spread at line _n_. Line
  \ _n_ and following lines are are moved down one line. Line
  \ _n_ becomes blank. Line 15 is lost.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`.
  \
  \ }doc

: h ( n -- ) line pad 1+ c/l dup pad c! cmove ; -->

  \ doc{
  \
  \ h ( n -- )
  \
  \ A command of `specforth-editor`: Hold line _n_ at `pad`
  \ (used by system more often than by user).
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`.
  \
  \ }doc

( specforth-editor )

: d ( n -- ) dup h $0F dup rot ?do i 1+ line i -move loop e ;

  \ doc{
  \
  \ d ( n -- )
  \
  \ A command of `specforth-editor`: Delete line _n_ but hold
  \ it in `pad`. Line 15 becomes free as all statements move up
  \ one line.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`.
  \
  \ }doc

: m ( n -- ) r# +! cr space #lead type '_' emit #lag type
#locate . drop ;
  \ doc{
  \
  \ m ( n -- )
  \
  \ A command of `specforth-editor`: Move the cursor by _n_
  \ characters. The position of the cursor on its line is shown
  \ by a "_" (underline).
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `-move`.
  \
  \ }doc

: t ( n -- ) dup c/l * r# ! dup h 0 m ;

  \ doc{
  \
  \ t ( n -- )
  \
  \ A command of `specforth-editor`: Type line _n_ and save in
  \ `pad`.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`.
  \
  \ }doc

: l ( -- ) scr @ list 0 m ;

  \ XXX TODO -- Make `l` common to both editors.

  \ doc{
  \
  \ l ( -- )
  \
  \ A command of `specforth-editor`: List the current block.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `scr`, `list`.
  \
  \ }doc

: r ( n -- ) pad 1+ swap -move ;

  \ doc{
  \
  \ r ( n -- )
  \
  \ A command of `specforth-editor`: Replace line _n_ with the
  \ text in `pad`.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `-move`.
  \
  \ }doc

: p ( n "ccc<eol>" -- ) text r ;

  \ doc{
  \
  \ p ( n "ccc<eol>" -- )
  \
  \ A command of `specforth-editor`: Put _ccc_ on line _n_.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `text`.
  \
  \ }doc

: i ( n -- ) dup s r ;

  \ doc{
  \
  \ i ( n -- )
  \
  \ A command of `specforth-editor`: Insert text from `pad` at
  \ line _n_, moving the old line _n_ down. Line 15 is lost.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`.
  \
  \ }doc

: clear ( n -- )
  scr ! l/scr 0 ?do [ also forth ] i [ previous ] e loop ;
  \ XXX TODO -- Simplify. Make it common to both editors.

  \ doc{
  \
  \ clear ( n -- )
  \
  \ A command of `specforth-editor`: Clear block _n_ with
  \ blanks and select for editing.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, e>>`, `l/scr`.
  \
  \ }doc

-->

( specforth-editor )

: -text ( ca1 len1 ca2 -- f )
  swap ?dup if over + swap ?do
                  dup c@ [ also forth ] i [ previous ] c@ -
                  if 0= leave else 1+ then
                loop else drop 0= then ;
  \ XXX TODO -- rewrite with `search`

  \ doc{
  \
  \ -text ( ca1 len1 ca2 -- f ) "minus-text"
  \
  \ Part of `specforth-editor`:
  \ Return a non-zero _f_ if string _ca1 len1_ exactly
  \ match string _ca2 len1_, else return a false flag.
  \
  \ See also: `match`.
  \
  \ }doc

: match ( ca1 len1 ca2 len2 -- true n3 | false n4 )
  >r >r 2dup r> r> 2swap over + swap [ also forth ]
  ?do 2dup i -text
    if >r 2drop r> - i swap - 0 swap 0 0 leave then
  loop [ previous ] 2drop swap 0= swap ;

  \ doc{
  \
  \ match ( ca1 len1 ca2 len2 -- true n3 | false n4 )
  \
  \ Part of `specforth-editor`:
  \ Match the string _ca2 len2_ with all strings contained in
  \ the string _ca1 len1_. If found leave _n3_ bytes until the
  \ end of the matching string, else leave _n4_ bytes to end of
  \ line.
  \
  \ See also: `-text`.
  \
  \ }doc

: 1line ( -- f ) #lag pad count match r# +! ;

  \ doc{
  \
  \ 1line ( -- f ) "1-line"
  \
  \ Part of `specforth-editor`:
  \ Scan the cursor line for a match to `pad` text. Return flag
  \ and update the cursor `r#` to the end of matching text, or
  \ to the start of the next line if no match is found.
  \
  \ See also: `#lag`, `match`.
  \
  \ }doc

: find ( -- )
  begin $03FF r# @ <
    \ XXX FIXME -- `00 error` ?
    if top pad here c/l 1+ cmove #-270 throw then 1line
  until ; -->

  \ doc{
  \
  \ find ( -- )
  \
  \ A command of `specforth-editor`:
  \ Search for a match to the string at `pad`, from the cursor
  \ position until the end of block.  If no match found issue
  \ an error message and reposition the cursor at the `top` of
  \ the block.
  \
  \ See also: `1line`.
  \
  \ }doc

( specforth-editor )

: delete ( n -- ) >r #lag + r@ - #lag r@ negate r# +! #lead +
                  swap cmove r> blank ;

  \ doc{
  \
  \ delete ( n -- )
  \
  \ A command of `specforth-editor`:
  \ Delete _n_ characters prior to the cursor.
  \
  \ See also: `#lag`, `r#`, `#lead`.
  \
  \ }doc

: n ( -- ) find 0 m ;

  \ doc{
  \
  \ n ( -- )
  \
  \ A command of `specforth-editor`: Find the next occurrence
  \ of the string found by an
  \ `<<src-lib-prog-editor-specforth-fs, f>>` command.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`,
  \ `<<src-lib-prog-editor-specforth-fs, find>>`.
  \
  \ }doc

: f ( "ccc<eol>" -- ) text n ;

  \ doc{
  \
  \ f ( "ccc<eol>" -- )
  \
  \ A command of `specforth-editor`: Search forward from the
  \ current cursor position until string _ccc_ is found. The
  \ cursor is left at the end of the string and the cursor line
  \ is printed. If the string is not found and error message is
  \ given and the cursor repositioned to the top of the block.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `text`.
  \
  \ }doc

: b ( -- ) pad c@ negate m ;

  \ doc{
  \
  \ b ( -- )
  \
  \ A command of `specforth-editor`: Used after
  \ `<<src-lib-prog-editor-specforth-fs, f>>` to backup the cursor by
  \ the length of the most recent text hold in `pad`.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`.
  \
  \ }doc

: x ( "ccc<eol>" -- ) text find pad c@ delete 0 m ;

  \ doc{
  \
  \ x ( "ccc<eol>" -- )
  \
  \ A command of `specforth-editor`: Find and delete the next
  \ occurrence of the string _ccc_.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, c>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`, `text`,
  \ `<<src-lib-prog-editor-specforth-fs, find>>`,
  \ `delete`.
  \
  \ }doc

: till ( "ccc<eol>" -- ) #lead + text 1line 0= #-270 ?throw
                           #lead + swap - delete 0 m ;
  \ doc{
  \
  \ till ( "ccc<eol>" -- )
  \
  \ A command of `specforth-editor`:
  \ Delete on the cursor line from the cursor till the end of
  \ string _ccc_.
  \
  \ }doc

: (c ( ca len -- )
  #lag rot over min >r r@ r# +! r@ - >r dup here r@ cmove
  here #lead + r> cmove r> cmove 0 m update ;

  \ XXX TODO -- rename

  \ doc{
  \
  \ (c ( ca len -- ) "paren-c"
  \
  \ Copy the string _ca len_ to the cursor line at the cursor
  \ position. ``(c`` is a factor of
  \ `<<src-lib-prog-editor-specforth-fs, c>>`.
  \
  \ See also: `#lag`, `r#`, `#lead`, `cmove`, `update`.
  \
  \ }doc

: c ( "ccc<eol>" -- )
  text pad count dup if (c else 2drop then ;

  \ doc{
  \
  \ c ( "ccc<eol>" -- )
  \
  \ A command of `specforth-editor`: Copy in _ccc_ to the
  \ cursor line at the cursor position.
  \
  \ See also: `<<src-lib-prog-editor-specforth-fs, b>>`,
  \ `<<src-lib-prog-editor-specforth-fs, d>>`,
  \ `<<src-lib-prog-editor-specforth-fs, e>>`,
  \ `<<src-lib-prog-editor-specforth-fs, f>>`,
  \ `<<src-lib-prog-editor-specforth-fs, h>>`,
  \ `<<src-lib-prog-editor-specforth-fs, i>>`,
  \ `<<src-lib-prog-editor-specforth-fs, l>>`,
  \ `<<src-lib-prog-editor-specforth-fs, m>>`,
  \ `<<src-lib-prog-editor-specforth-fs, n>>`,
  \ `<<src-lib-prog-editor-specforth-fs, p>>`,
  \ `<<src-lib-prog-editor-specforth-fs, r>>`,
  \ `<<src-lib-prog-editor-specforth-fs, s>>`,
  \ `<<src-lib-prog-editor-specforth-fs, t>>`,
  \ `<<src-lib-prog-editor-specforth-fs, x>>`, `(c`, `text`.
  \
  \ }doc

: copy ( n1 n2 -- ) swap block cell- ! update save-buffers ;

  \ doc{
  \
  \ copy ( n1 n2 -- )
  \
  \ A command of `specforth-editor`:
  \ Copy `block` _n1_ to block _n2_.
  \
  \ See also: `update`, `save-buffers`.
  \
  \ }doc

only forth definitions

  \ ===========================================================
  \ Change log

  \ 2015-09-11: Adapted to Solo Forth.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the library.
  \
  \ 2016-05-14: Update with `parse-all`, a fixed version of old
  \ `parse-line`.
  \
  \ 2016-05-18: Need `vocabulary`, which has been moved to the
  \ library.
  \
  \ 2016-08-05: Compact the code to save two blocks.
  \
  \ 2016-11-21: Complete and improve documentation of all words. Use
  \ radix prefix instead of `hex`. Init `r#`. Use `l/scr` instead of a
  \ literal.
  \
  \ 2016-11-22: Move `r#` <editor.common.fsb>` and get `top` from it.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the library,
  \ with `?do`.
  \
  \ 2017-02-24: Add cross references to documentation.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to words that
  \ need it.
  \
  \ 2018-02-27: Update source style (remove double spaces).
  \
  \ 2018-06-04: Update: remove trailing closing paren from word names.
  \
  \ 2020-05-03: Add `specforth-editor`.
  \
  \ 2020-05-05: Remove old unused version of `text`.
  \
  \ 2020-05-06: Document `specforth-editor`. Improve documentation.
  \
  \ 2020-05-10: Improve documentation.
  \
  \ 2020-05-18: Fix documentation markup.
  \
  \ 2020-05-18: Update: `+loop` was moved to the library.
  \
  \ 2020-05-24: Load `r#` and `top` in `forth-wordlist`, because they
  \ are shared with `gforth-editor`. Replace "hash" notation with
  \ "number sign".
  \
  \ 2020-05-26: Improve documentation. Make links to `find` explicit
  \ (there's also the homonym standard word).
  \
  \ 2020-07-10: Improve layout of tables.
  \
  \ 2020-07-11: Modify markup of table title.
