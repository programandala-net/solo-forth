  \ strings.substitute.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Forth-2012's `substitute`.

  \ ===========================================================
  \ Author

  \ Unknown. Published in the documentation of Forth-2012.
  \
  \ Marcos Cruz (programandala.net) adapted it to Solo Forth,
  \ 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( substitute )

need find-substitution need 2variable

'%' cconstant substitution-delimiter
  \ Character used as the substitution name delimiter.

  \ doc{
  \
  \ substitution-delimiter ( -- c )
  \
  \ A character constant that returns the character used as
  \ delimiter by `substitute`. By default it's "%".
  \
  \ See also: `substitution-delimiter?`.
  \
  \ }doc

variable /substitute-result
  \ Maximum length of the destination buffer.

2variable substitute-result
  \ Destination string current length and address.

variable substitute-error
  \ Zero or an error code.

: c>substitute-result ( c -- )
  substitute-result @ /substitute-result @ <
  if    substitute-result 2@ + c! 1 chars substitute-result +!
  else  drop #-78 substitute-error !  then ;
  \ Add the character _c_ to the destination string.
  \ XXX TODO -- use a specific exception code

: str>substitute-result ( ca len -- )
  bounds ?do  i c@ c>substitute-result  loop ;  -->

  \ Add a string to the output string.
  \ XXX TODO -- faster, not character by character

( substitute )

: get-substitution ( ca1 len1 -- ca3 len3 ca2 len2 )
  1/string 2dup substitution-delimiter scan dup
  if    dup >r 1/string 2swap r> -
  else  2swap #-78 substitute-error !  then ;
  \ Given a source string _ca1 len1_ pointing at a leading
  \ delimiter, divide into the delimited substitution _ca2
  \ len2_ and the residue _ca3 len3_ of the string
  \
  \ If no ending delimiter can be found, `substitute-error` is
  \ updated, the substitution is the whole string after the
  \ starting delimiter, and the residue is an empty string.
  \
  \ XXX TODO -- use a specific exception code

: substituted? ( ca len -- f )
  2dup find-substitution dup >r
  if    execute str>substitute-result 2drop
  else  substitution-delimiter c>substitute-result
        str>substitute-result
        substitution-delimiter c>substitute-result  then  r> ;
  \ Process the substitution _ca len_. Return `true` if
  \ found and substituted; return `false` if not found.

need >body

code substitution-delimiter? ( ca -- f )
  E1 c, 3A c, ' substitution-delimiter >body , BE c,
  \ pop hl
  \ ld a,(substitute_delimiter)
  \ cp (hl)
  CA c, ' true , C3 c, ' false , end-code  -->
  \ jp z,true
  \ jp false

  \ doc{
  \
  \ substitution-delimiter? ( ca -- f ) "substitution-delimiter-question"
  \
  \ Does _ca_ contains the character hold in the character
  \ constant `substitution-delimiter`? If so return `true`,
  \ else return `false`.
  \
  \ ``substitution-delimiter?`` is a factor of `substitute`.
  \
  \ ``substitution-delimiter?`` is written in Z80. Its
  \ equivalent definition is Forth is the following:

  \ ----
  \ : substitution-delimiter? ( ca -- f )
  \  c@ substitution-delimiter = ;
  \ ----

  \ }doc

( substitute )

: substitute ( ca1 len1 ca2 len2 -- ca2 len3 n )
   /substitute-result ! 0 substitute-result 2! 0 -rot
   \ ( -- 0 ca1 len1 )
   substitute-error off
   begin  dup 0>  while ( -- n ca1 len1 )
     over substitution-delimiter? if
       over char+ substitution-delimiter?
       if    substitution-delimiter c>substitute-result
             2 /string
       else  get-substitution
             substituted? if  rot 1+ -rot  then
       then
     else  over c@ c>substitute-result 1/string  then
   repeat  2drop substitute-result 2@ rot
           substitute-error @ ?dup if  nip  then ;

  \ doc{
  \
  \ substitute ( ca1 len1 ca2 len2 -- ca2 len3 n )

  \
  \ Perform substitution on the string _ca1 len1_ placing
  \ the result at string _ca2 len3_, where _len3_ is the length
  \ of the resulting string. An error occurs if the resulting
  \ string will not fit into _ca2 len2_ or if _ca2_ is
  \ the same as _ca1_. The return value _n_ is positive or 0
  \ on success and indicates the number of substitutions made.
  \ A negative value for _n_ indicates that an error occurred,
  \ leaving _ca2 len3_ undefined, and being _n_ the exception
  \ code.
  \
  \ Substitution occurs left to right from the start of
  \ _ca1_ in one pass and is non-recursive.  When text  of
  \ a  potential substitution  name, surrounded  by "%" (ASCII
  \ $25)  delimiters is  encountered  by `substitute`, the
  \ following occurs:
  \
  \  1. If the name is null, a single delimiter character is
  \  passed to the output, i.e., "%%" is replaced by "%". The
  \  current number of substitutions is not changed.
  \
  \  2. If the text is a valid substitution name acceptable to
  \  `replaces`, the leading and trailing
  \  delimiter characters and the enclosed substitution name
  \  are replaced by the substitution text. The current number
  \  of substitutions is incremented.
  \
  \  3. If the text is not a valid substitution name, the name
  \  with leading and trailing delimiters is passed unchanged
  \  to the output. The current number of substitutions is not
  \  changed.
  \
  \  4. Parsing of the input string resumes after the trailing
  \  delimiter.
  \
  \ If after processing any pairs of delimiters, the residue of
  \ the input string contains a single delimiter,  the residue
  \ is passed unchanged to the output.
  \
  \ See also: `unescape`, `substitution-delimiter?`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-01-21: First version, adapted from the documentation
  \ of Forth-2012.
  \
  \ 2017-01-22: Improve documentation. Make the code more clear
  \ by renaming and a bit of factoring. Make it faster with Z80
  \ code. Rewrite some parts to make the intermediate buffer
  \ unnecessary.
  \
  \ 2017-01-23: Modify to support the new alternative
  \ `xt-replaces`: Now execution of a substitution returns the
  \ string pair, not the address of a counted string. Update
  \ with `1/string`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2020-05-19: Update: `2variable` has been moved to the
  \ library.
  \
  \ 2020-06-08: Improve documentation: make _true_ and
  \ _false_ cross-references.

  \ vim: filetype=soloforth
