\ Solo Forth
\

\ *******************************************************
\ XXX WARNING

\ This is unfinished experimental code!

\ This source is being converted from GNU binutils to my own Z80 Forth
\ assembler under development (<~/forth/okdek/).

\ 2015-08-18: Start.

\ The main problem is how to manage the undefined symbols. Many of
\ them could be solved by rearranging the source code.

\ *******************************************************

  .text

\ XXX TODO
'A' constant version_status
00 constant version_branch
2015081721 constant version_release

\ XXX TMP -- for debugging
\ vr_div: equ version_release/65535
\ vr_mod: equ version_release mod 65535
\ vr_mod2: equ version_release - vr_div

\ A Forth system for ZX Spectrum 128K and G+DOS.
\ http://programandala.net/en.program.solo_forth.html

\ Copyright (C) 2015 Marcos Cruz (programandala.net)

\ Copying and distribution of this file, with or without
\ modification, are permitted in any medium without royalty
\ provided the copyright notice, the aknowledgments file and
\ this notice are preserved.  This file is offered as-is,
\ without any warranty.

\ ==============================================================
\ Acknowledgments

\ See the file <ACKNOWLEDGMENTS.adoc>.

\ ==============================================================
\ History

\ See
\ http://programandala.net/en.program.solo_forth.history.html

\ ==============================================================
\ System description

\ ----------------------------------------------
\ Forth Registers

\ Forth Z80  Forth preservation rules
\ ----- ---  ------------------------
\ IP    BC   Interpretive pointer.
\            Should be preserved across Forth words.
\ SP    SP   Data stack pointer.
\            Should be used only as data stack across Forth words.
\            May be used within Forth words if restored before exit.
\       DE   Input only when pushhlde called. \ XXX TODO
\       HL   Input only when pushhl called. \ XXX TODO
\       IX   Address of `next`.
\            May be used within Forth words if restored before exit.
\       IY   Address of the ERRNR ZX Spectrum system variable.
\            May be used within Forth words if restored before exit.

\ ----------------------------------------------
\ Header structure

\ The name and link fields are created in a memory bank:

\ cfap: dw cfa             \ Pointer to cfa in main memory.
\ lfa:  dw nfa of the previous word
\ nfa:  db length+flags    \ Bits:      76543210
                           \ Bit names: .PSLLLLL
                           \ Legend:
                           \   P: Precedence bit.
                           \      0 = non-immediate word
                           \      1 = immediate word
                           \   S: Smudge bit:
                           \      0 = definition completed
                           \      1 = definition not completed
                           \   LLLLL: name length (0..31).
\       ds length          \ name

\ The code and parameter fields are created in the dictionary:

\ cfa: dw code_address
\ pfa:    ...              \ data or code

\ ==============================================================
\ Glossary

\ The description of Forth words is included in this source.
\ The markers `doc{` and `}doc` delimitate the comments that
\ form the glossary.

\ ----------------------------------------------
\ Stack notation

\ XXX TODO
\ XXX TODO update when true=-1

\ a        = address
\ ca       = character-aligned address

\ f        = flag (false is 0; true is any other value)
\ tf       = true flag (1)
\ ff       = false flag (0)
\ wf       = well-formed flag (false is 0; true is 1)

\ b        = 8-bit byte
\ c        = 7-bit or 8-bit character
\ u        = 16-bit unsigned number
\ len      = 16-bit unsigned number, length of memory zone or string
\ ca len   = string
\ n        = 16-bit signed number
\ x        = 16-bit signed or unsigned number
\ d        = 32-bit signed double number
\ ud       = 32-bit unsigned double number
\ xd       = 32-bit signed or unsigned number

\ xc       = 8-bit graphic x coordinate (0..255)
\ yc       = 8-bit graphic y coordinate (0..191)
\ line     = 8-bit cursor line (0..23)
\ col      = 8-bit cursor column (0..31)

\ cfa      = code field address
\ lfa      = link field address
\ nfa      = name field address
\ pfa      = parameter field address
\ cfap     = code field address pointer

\ orig     = address of an unresolved forward branch
\ dest     = address of a backward branch target

\ cs-id    = control structure identifier

\ op       = Z80 8-bit opcode, generally a jump
\ r        = Z80 8-bit register identifier
\ rp       = Z80 16-bit register pair identifier

\ ----------------------------------------------
\ Parsed text notation

\ XXX TODO

\ <char>          the delimiting character marking the end of the
\                 string being parsed
\ <chars>         zero or more consecutive occurrences of the
\                 character char
\ <space>         a delimiting space character
\ <spaces>        zero or more consecutive occurrences of the
\                 character space
\ <quote>         a delimiting double quote
\ <paren>         a delimiting right parenthesis
\ <eol>           an implied delimiter marking the end of a line
\ ccc             a parsed sequence of arbitrary characters,
\                 excluding the delimiter character
\ text            same as ccc
\ name            a token delimited by space, equivalent to
\                 ccc<space> or ccc<eol>

\ ----------------------------------------------
\ Word attributes

\ XXX TODO -- finish

\ The capital letters on the right show definition characteristics:

\ C      May only be used within a colon definition. A digit indicates number
\        of memory addresses used, if other than one. A plus sign indicates
\        a variable number of memory addresses used.
\ E      Intended for execution only.
\ I      Immediate. Has precedence bit set. Will execute even when compiling.
\ U      A user variable.

\ ==============================================================
\ Configuration

  \ XXX FIXME Pasmo gives strange errors (symbols not found)
  \ when some config flags are used in nested `if`. A literal
  \ flag (0/1) is used instead, with the flag name in a comment;
  \ it is changed with a text substitution.  Some Vim mappings
  \ are created to turn them on on an off.

  \ XXX experimental
false constant size_optimization?
  \ true = some code pieces are more compact but slower.
  \ false = normal, faster code.

  \ XXX TODO -- not used yet
false constant fig_parsing?
  \ true = the fig-Forth parsing method is used.
  \ false = parsing is modified after Forth-83 and ANS Forth.

  \ XXX TODO
false constant latin1_charset_in_bank?
  \ true = a 224-char Latin 1 charset is stored in the memory bank.
  \ false = the default charset is used.

false constant ans_forth_block_size?
  \ true = one 1024-byte block per screen \ XXX TODO
  \ false = two 512-byte blocks per screen

  \ XXX TODO
true constant fig_exit?
  \ true = fig-Forth `;s` is used
  \ false = Forth-83 and ANS Forth `exit` is used \ XXX FIXME

  \ XXX FIXME still there are problems when compiler security is off
true constant fig_compiler_security?
  \ true = fig-Forth `?pairs' is used
  \ false = no checking during compilation of control
  \   structures: smaller and faster code.

  \ XXX TODO
false constant show_version?

\ ==============================================================
\ Symbols

\ ----------------------------------------------
\ Forth

0x5E00 constant origin

1 constant true_flag \ XXX TMP untill true=-1
true_flag -1 = [if]
  \ XXX FIXME
  \ There were problems when true=-1, but it seems they have
  \ disappeared after switching to the one buffer method.
  .warning TRUE is -1
[then]

2 constant cell

0x50 constant cells_per_data_stack
0x50 constant cells_per_return_stack

0x50 constant bytes_per_terminal_input_buffer

0x01 constant buffers

ans_forth_block_size? [if]

0x0400 constant data_bytes_per_buffer
0x01 constant blocks_per_screen

[else]

0x0200 constant data_bytes_per_buffer
0x02 constant blocks_per_screen

[then]

2 constant total_bytes_per_buffer+data_bytes_per_buffer+3

0x030C constant screens_per_disk \ 780 KiB per disk in G+DOS
0x40 constant characters_per_line
0x10 constant lines_per_screen
0x08 constant max_search_order \ maximum number of vocabularies in the search order
0x40 constant bytes_per_user_variables

6 constant precedence_bit
1 constant precedence_bit_mask << precedence_bit
5 constant smudge_bit
1 constant smudge_bit_mask << smudge_bit

0x1F constant max_word_length
max_word_length constant max_word_length_bit_mask

256 constant csb_size \ size of the circular string buffer

\ Memory banks

0 constant default_bank
1 constant names_bank

0xC000 constant names_bank_address \ names pointers

\ Charset

224 constant charset_size*8 \ 224 chars (0x20..0xFF) * 8 bitmap rows
0xFFFF constant charset_address-charset_size+1

\ Control structure check numbers

\ XXX TODO -- not used yet
\ begin_structure_check_number:   equ 1
\ if_structure_check_number:      equ 2
\ do_structure_check_number:      equ 3
\ case_structure_check_number:    equ 4
\ of_structure_check_number:      equ 5
\ for_structure_check_number:     equ 6

\ Error messages are in the disk, starting from the screen
\ number hold in the `msg-scr` constant.  Error codes 0, 16, 32
\ etc are not used, because they coincide with the first line of
\ screens.

\ XXX TODO change the order

01 constant error.not_understood
02 constant error.stack_empty
03 constant error.dictionary_full \ not used
04 constant error.not_unique
05 constant error.not_found
06 constant error.out_of_disk_range
07 constant error.full_stack
08 constant error.number_08 \ free
09 constant error.loading_from_screen_0
10 constant error.number_10 \ free
11 constant error.number_11 \ free
12 constant error.number_12 \ free
13 constant error.number_13 \ free
14 constant error.number_14 \ free
15 constant error.deferred_word_uninitialized
17 constant error.compilation_only
18 constant error.execution_only
19 constant error.conditionals_not_paired
20 constant error.definition_not_finished
21 constant error.protected_dictionary
22 constant error.loading_only
23 constant error.off_current_editing_screen
24 constant error.declare_vocabulary
25 constant error.unsupported_tape_operation
26 constant error.unsupported_disk_operation
27 constant error.source_file_needed
28 constant error.not_present_though_required
29 constant error.required_but_not_located
30 constant error.branch_too_long
31 constant error.number_31 \ free
32 constant error.number_33 \ free

\ ----------------------------------------------
\ Character codes

0x06 constant caps_char \ toggle caps lock
0x07 constant edit_char \ edit
0x08 constant backspace_char
0x0c constant delete_char \ delete (backspace)
0x0c constant form_feed_char \ used for printing
0x0d constant carriage_return_char
0x0e constant extended_mode_char \ Fuse associates it to the host's Tab key
0x0f constant graphics_char \ toggle graphics mode
0x10 constant ink_char
0x11 constant paper_char
0x12 constant flash_char
0x13 constant bright_char
0x14 constant inverse_char
0x15 constant over_char
0x16 constant at_char
0x17 constant tab_char \ tab (screen only)
0x20 constant space_char
0x7F constant copyright_char

\ ----------------------------------------------
\ ROM  routines

0x1601 constant rom_chan_open
\ rom_cl_all:                      equ 0x0DAF \ XXX OLD
0x0333 constant rom_key_decode
0x028E constant rom_key_scan
0x031E constant rom_key_test
0x1CAD constant rom_set_permanent_colors_0x1CAD

\ ----------------------------------------------
\ System variables

  \ XXX FIXME Pasmo's bug?: `sys_errnr` is used as the base offset.  When it's
  \ not defined first, the compilation halts with error "offset out of
  \ range", though they are fine in the symbols file.

0x5C3A constant sys_errnr \ used as IY index by the OS

0x5C8D constant sys_attr_p
0x5C8F constant sys_attr_t
0x5B5C constant sys_bankm
0x5C48 constant sys_bordcr
0x5C36 constant sys_chars
0x5C84 constant sys_df_cc
0x5C6B constant sys_df_sz
sys_df_sz constant sys_df_sz_offset sys_errnr -
0x5C6A constant sys_flags2
0x5C08 constant sys_last_k
sys_last_k constant sys_last_k_offset sys_errnr -
0x5C41 constant sys_mode
0x5C88 constant sys_s_posn
0x5C8C constant sys_scr_ct
sys_scr_ct constant sys_scr_ct_offset sys_errnr -
0x5C7B constant sys_udg

\ ----------------------------------------------
\ System constants

0x4000 constant sys_screen
0x1B00 constant sys_screen_size
0x1800 constant sys_screen_bitmap_size
0x5800 constant sys_screen_attributes
0x0300 constant sys_screen_attributes_size

\ ----------------------------------------------
\ Ports

0x7FFD constant bank1_port
0xFE constant border_port

\ ==============================================================
\ Entry points

  .text

$ constant cold_entry
  \ Location (of the destination address): `0x01 +origin`
  jp cold_start
$ constant warm_entry
  \ Location (of the destination address): `0x04 +origin`
  jp warm_start

\ ==============================================================
\ Parameter area

  \ XXX TODO document the `+origin` index

$ constant latest_nfa_in_root_voc.init_value
  latest_nfa_in_root_voc __

$ constant latest_nfa_in_forth_voc.init_value
  latest_nfa_in_forth_voc __

$ constant latest_nfa_in_assembler_voc.init_value
  latest_nfa_in_assembler_voc __

$ constant voc_link.init_value
  assembler_vocabulary_link __ \ link to the latest vocabulary defined

$ constant user_variables_pointer
  user_variables __

$ constant return_stack_pointer
  return_stack_bottom __

$ constant default_color_attribute
  4 __ \ low byte: green paper, black ink; high byte: no mask

  \ XXX TODO
show_version? [if]
$ constant version_status_variable
  version_status __
$ constant version_branch_variable
  version_branch __
$ constant version_release_variable
  version_release 0xFFFF mod __
  version_release 0xFFFF / __
[then]

  \ User variables default values

  \ The first eight user variables have default values.  They are used
  \ by `cold` to overwrite the correspondent user variables.  They must
  \ be in the same order than user variables.

$ constant default_user_variables_start

$ constant s0_init_value
  data_stack_bottom __
$ constant r0_init_value
  return_stack_bottom __
  0x0000 __ \ XXX OLD -- tib
$ constant width_init_value
  max_word_length __
$ constant warning_init_value
  0x0000 __
  0x0000 __ \ XXX OLD -- fence
$ constant dp_init_value
  dictionary_pointer_after_cold __

  \ XXX TODO move
  0x0000 __ \ XXX free

  \ XXX TODO this four user variables do not need init and this
  \ space could be saved; they are included here because `#tib`
  \ must be init; it should be moved to user variable +0x10.

$ constant blk_init_value
  0x0000 __
$ constant in_init_value
  0x0000 __
$ constant out_init_value
  0x0000 __
$ constant scr_init_value
  0x0000 __

\ XXX OLD
\ number_tib_init_value:
\  bytes_per_terminal_input_buffer __

$ constant default_user_variables_end

ip_backup: \ temporary copy of Forth IP
  0 __

\ XXX FIXME binutils bug?
\ With this `defl`, np doesn't change its constant (0xC001);
\ without it, it keeps the first constant assigned with `defl`
\ in the `_header` macro (0xC00A).
\ np defl names_bank_address+1

np defl 0xC000 \ data_start

$ constant names_pointer
  \ First free address in the names bank,  restored by `cold`.
  np __

$ constant names_pointer_init_value
  \ Init constant of the names pointer, used by `cold`.
  np __

\ ==============================================================
\ User variables

$ constant user_variables

  \ Note: the first eight user variables are initialized with
  \ default values by `cold`.  They must be in the same order
  \ than their default variables.

s0_value: \ +0x00
  data_stack_bottom __
r0_value: \ +0x02
  return_stack_bottom __
  \ +0x04
  0x0000 __ \ XXX OLD -- tib
width_value: \ +0x06
  max_word_length __
warning_value: \ +0x08
  0x0000 __
  \ +0x0A
  0x0000 __ \ XXX OLD -- fence
dp_value: \ +0x0C
  dictionary_pointer_after_cold __

  \ XXX TODO move
  0x0000 __ \ +0x0E free

blk_value: \ +0x10
  0x0000 __
in_value: \ +0x12
  0x0000 __
out_value: \ +0x14
  0x0000 __
scr_value: \ +0x16
  0x0000 __
number_tib_value: \ +0x18
  0x0000 __
hld_value: \ +0x1A
  0x0000 __
current_value: \ +0x1C
  0x0000 __
state_value: \ +0x1E
  0x0000 __
base_value: \ +0x20
  0x000A __
dpl_value: \ +0x22
  0x0000 __
fld_value: \ +0x24
  0x0000 __
csp_value: \ +0x26
  0x0000 __
r_hash_value: \ +0x28 ; XXX OLD -- used by the editor, remove
  0x0000 __

context_value: \ +0x2A..+0x38

  forth_pfa __
  root_pfa __
  ds (max_search_order-2)*cell
  0x0000 __ \ end of search order, required by `find` \ XXX TODO improve and remove

  \ Unused
  0x0000 __
  0x0000 __

($-user_variables) != bytes_per_user_variables [if]
  .error "The space reserved for user variables is wrong."
[then]

\ ==============================================================
\ Stacks and buffers

\ ----------------------------------------------
\ Circular string buffer

$ constant csb
$ constant unused_csb
  csb_size __ \ unused space in the buffer
$ constant csb0
  ds csb_size
csb_total_size: equ $-csb

\ ----------------------------------------------
\ Data stack

data_stack_limit: equ $+cell
  ds cells_per_data_stack*cell
$ constant data_stack_bottom

\ ----------------------------------------------
\ Terminal input buffer

$ constant terminal_input_buffer
  ds bytes_per_terminal_input_buffer
  ds 3 \ for the null word

\ ----------------------------------------------
\ Return stack

return_stack_limit: equ $+cell
  ds cells_per_return_stack*cell
$ constant return_stack_bottom

\ ----------------------------------------------
\ Disk buffer

0x7FFF constant buffer_block_id_mask

\ A block id is the number of the associated block, with the
\ sign bit indicating, when it's set, that the buffer has been
\ modified.

$ constant disk_buffer

buffer_block_id_mask __     \ Block id used when the  buffer
                            \ is not associated with a block.
ds data_bytes_per_buffer    \ Actual content of the block,
                            \ a disk sector.
space_char _ 0 _ space_char _  \ Null word, required by the parsing words.

\ ==============================================================
\ Macros

\ ----------------------------------------------
\ Header

precedence_bit_mask constant immediate \ used as optional parameter

nfa_of_the_previous_word defl 0 \ link to previous Forth word
\ current_vocabulary defl forth_pfa \ XXX OLD

_header: macro _base_label,_name,_is_immediate=0

  \ In dictionary:

  .text

\_base_label: \ code field address

  \ In memory bank:

  .data

\_base_label\()cfap: 
    \_base_label __ \ code field address pointer
\_base_label\()lfa: \ link field address
    nfa_of_the_previous_word __ \ link field
\_base_label\()nfa: \ name field address

  \ Length byte with optional precedence bit:
  _address_after_name _name_address - \_is_immediate + _

$ to _name_address
  ,sz "\_name" \ name field \ XXX TODO
$ to _address_after_name

\ np defl $ \ update the names pointer ; XXX does not work
$ to np \ XXX it works! the symbol is updated and also appears in the list

  \ In dictionary:
  
  .text

nfa_of_the_previous_word defl \_base_label\()nfa

  endm

_code_header: macro _base_label,_name,_is_immediate=0

  _header \_base_label,"\_name",\_is_immediate
  \_base_label\()pfa __ \ code field
  \_base_label\()pfa: \ parameter field address

  endm

_code_alias_header: macro _base_label,_name,_is_immediate=0,_alias

  _header \_base_label,"\_name",\_is_immediate
  \_alias\()pfa __ \ code field

  endm

_colon_header: macro _base_label,_name,_is_immediate=0

  _header \_base_label,"\_name",\_is_immediate
  do_colon __ \ code field
  \_base_label\()pfa: \ parameter field address

  endm

_user_variable_header: macro _base_label,_name,_is_immediate=0

  _header \_base_label,"\_name",\_is_immediate
  do_user __ \ code field
  \_base_label\()pfa: \ parameter field address

  endm

_does_header: macro _base_label,_name,_is_immediate=0,_runtime_routine

  _header \_base_label,"\_name",\_is_immediate

  \_runtime_routine __ \ code field
  \_base_label\()pfa: \ parameter field address

  endm

_constant_header: macro _base_label,_name,_is_immediate=0

  _header \_base_label,"\_name",\_is_immediate
  do_constant __ \ code field
  \_base_label\()pfa: \ parameter field address

  endm

_variable_header: macro _base_label,_name,_is_immediate=0

  _header \_base_label,"\_name",\_is_immediate
  do_create __ \ code field
  \_base_label\()pfa: \ parameter field address

  endm

_two_variable_header: macro _base_label,_name,_is_immediate=0

  _variable_header \_base_label,"\_name",\_is_immediate

  endm

\ ----------------------------------------------
\ Literals

_string: macro _text

  \ XXX TODO
  \ XXX FIXME binutils compiles wrong lengths
  
  .warning "The _string macro is obsolete"

  _string_end $ - 1- \ length byte
  \ db "\_text" \ 
_string_end defl $

endm

: _literal  ( n -- )

  \ XXX TODO move after the needed definitions

  ?dup if  zero_ __ exit  then
  dup 1 = if  drop one_ __ exit  then
  dup 2 = if  drop two_ __ exit  then
  dup 0 >= over 255 <= and if  c_lit_ __ _ exit  then
  lit_ __ __  ;

\ ----------------------------------------------
\ Jumps

' jpix alias jpnext

\ Create relative or absolute jumps, depending on the configured optimization

_jump: macro _address
  size_optimization? [if]
    jr \_address
  [else]
    jp \_address
  [then]
  endm

_jump_nc: macro _address
  size_optimization? [if]
    jr nc,\_address
  [else]
    jp nc,\_address
  [then]
  endm

_jump_z: macro _address
  size_optimization? [if]
    jr z,\_address
  [else]
    jp z,\_address
  [then]
  endm

\ ----------------------------------------------
\ Bank

: _bank  ( n -- )  _literal bank_ __  ;

: _names_bank  ( -- )  names_bank _bank  ;

: _default_bank  ( -- )  default_bank _bank  ;

\ ----------------------------------------------
\ Error messages

_question_error: macro _error
  \_error _literal
  question_error_ __
  endm

_message: macro _error
  \_error _literal
  message_ __
  endm

\ ----------------------------------------------
\ Debug

: _z80_border  ( color -- )
  af push
  bc push
  a ld \ color
  border_port out
  0 bc ldp#
  herez
    bc decp
    b a ld
    c or
  jrnz
  bc pop
  af pop
  ;

: _z80_border_wait ( color -- )
  af push
  ld a,\_color
  out (border_port),a
  a xor
  ld (sys_last_k),a
_z80_border_wait_pause: defl $
  ld a,(sys_last_k)
  a and
  jr z,_z80_border_wait_pause
  af pop
  ;

_echo: macro _txt
  .warning "The _echo macro is obsolete"
  cr_ __ paren_dot_quote_ __
  _string "\_txt"
  endm

\ ==============================================================
\ Misc routines

\ [Code from DZX-Forth.]

\ ----------------------------------------------
\ Compare de and hl

$ constant compare_de_hl_unsigned

  \ Input:  de, hl
  \ Output:
  \  flag C if hl < de
  \  flag Z if hl = de

  h a ld
  cp d
  retnz
  l a ld
  cp e
  ret

$ constant compare_de_hl_signed

  \ Input:  de, hl
  \ Output: flag C if hl < de

  h a ld
  d xor
  jp p,compare_de_hl_unsigned
  h a ld
  a or
  retp
  scf
  ret

\ ----------------------------------------------
\ Move block

$ constant move_block

  \ Input:
  \ hl = source
  \ de = destination
  \ bc = count

  \ If bc is greater than zero, copy the contents of bc consecutive address
  \ units at hl to the bc consecutive address units at de. After the move
  \ completes, the bc consecutive address units at de contain exactly what the
  \ bc consecutive address units at hl contained before the move.

  compare_de_hl_unsigned call
  jp c,move_block_downwards

\ ----------------------------------------------
\ Move block upwards

$ constant move_block_upwards

  \ Input:
  \ hl = source
  \ de = destination
  \ bc = count

  \ If bc is greater than zero, copy bc consecutive characters from the data
  \ space starting at hl to that starting at de, proceeding
  \ character-by-character from lower addresses to higher addresses.

  c a ld
  b or
  retz
  ldir
  ret

\ ----------------------------------------------
\ Move block downwards

  \ Input:
  \ hl = source
  \ de = destination
  \ bc = count

  \ If bc is greater than zero, copy bc consecutive characters from the data
  \ space starting at hl to that starting at de, proceeding
  \ character-by-character from higher addresses to lower addresses.

$ constant move_block_downwards

  c a ld
  b or
  retz
  add hl,bc
  hl decp
  ex de,hl
  add hl,bc
  hl decp
  ex de,hl
  lddr
  ret

\ ----------------------------------------------
\ Multiplication primitives

\ AHL <- A * DE

$ constant a_multiplied_by_de_to_ahl
  ld hl,0
  ld c,8
$ constant a_multiplied_by_de_to_ahl.1
  add hl,hl
  rla
  jp nc,a_multiplied_by_de_to_ahl.2
  add hl,de
  adc a,0
$ constant a_multiplied_by_de_to_ahl.2
  c dec
  jp nz,a_multiplied_by_de_to_ahl.1
  ret

\ Unsigned 16*16 multiply, 32-bit result

\ HLDE <- HL * DE

$ constant hl_multiplied_by_de_to_hlde_unsigned
  bc push \ save Forth IP
  h b ld
  l a ld
  a_multiplied_by_de_to_ahl call
  hl push
  a h ld
  b a ld
  h b ld
  a_multiplied_by_de_to_ahl call
  de pop
  d c ld
  add hl,bc
  adc a,0
  l d ld
  h l ld
  a h ld
  bc pop \ restore Forth IP
  ret

\ ==============================================================
\ :Inner interpreter

$ constant push_hlde
  de push

$ constant push_hl
  hl push

$ constant next
  \ Execute the word whose cfa is in the address pointed by the bc register.
  \ Forth: W  <-- (IP)
  \ Z80:   hl <-- (bc)
  ld a,(bc)
  a l ld
  bc incp \ inc IP
  ld a,(bc)
  a h ld
  bc incp \ inc IP
  \ bc = address of the next cfa
  \ hl = cfa

$ constant next2
  \ Execute the word whose cfa is in the hl register.
  \ Forth: PC <-- (W)
  \ Z80:   pc <-- (hl)
  ld e,(hl)
  hl incp
  ld d,(hl)
  ex de,hl
  \ hl = (cfa) = address of the code
  \ de = cfa+1 = pfa-1

next2_end: \ XXX TMP for debugging
  jp (hl)

\ ==============================================================
\ Dictionary

\ ----------------------------------------------
\ Start compiling in the `root` vocabulary

nfa_of_the_previous_word defl 0 \ link to previous Forth word
\ current_vocabulary defl root_pfa

\ ----------------------------------------------
  _code_header root_x_,"\x00",immediate

\ doc{

\ x  ( -- )

\ This is a pseudonym for an alias of the "null" word that is
\ defined in the `forth` vocabulary.

\ }doc

  ld hl,x_ \ cfa of the actual null word
  jp next2 \ execute it

\ ----------------------------------------------
  _colon_header root_forth_,"FORTH"

  forth_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header root_definitions_,"DEFINITIONS"

  definitions_ __
  semicolon_s_ __

root_definitions_nfa constant latest_nfa_in_root_voc

\ ----------------------------------------------
\ Start compiling in the `assembler` vocabulary

nfa_of_the_previous_word defl 0 \ link to previous Forth word
\ current_vocabulary defl forth_pfa

\ ----------------------------------------------
  _variable_header abase_,"ABASE"

\ doc{
\
\ abase  ( -- a )
\
\ A variable used to save the current constant of `base` in
\ assembler definitions.
\
\ }doc

  0 __

\ ----------------------------------------------
  _colon_header asm_,"ASM"

\ doc{
\
\ asm  ( -- )
\
\ Enter the assembler mode.
\
\ }doc

  \ [Idea taken from Coos Haak's Z80 Forth assembler.]

  noop_ __ \ to be patched by the assembler
  base_ __ fetch_ __ abase_ __ store_ __ \ save the current base
  hex_ __
  also_ __ assembler_ __ \ XXX TODO better
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header end_asm_,"END-ASM"

\ doc{
\
\ end-asm  ( -- )
\
\ Exit the assembler mode.
\
\ }doc

  previous_ __ \ restore the search order ; XXX TODO better
  abase_ __ fetch_ __ base_ __ store_ __ \ restore `base`
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header end_code_,"END-CODE"

  question_csp_ __ end_asm_ __ smudge_ __
  semicolon_s_ __

\ ----------------------------------------------
  _constant_header next_,"NEXT"

  next __

\ ----------------------------------------------
  \ _constant_header next_,"NEXT2"

  \ \ XXX OLD -- added for the second version of `defer`, but
  \ \ not needed
  
  \ next2 __

\ ----------------------------------------------
  _constant_header pushhl_,"PUSHHL"

  push_hl __

\ ----------------------------------------------
  _constant_header pushhlde_,"PUSHHLDE"

  push_hlde __

\ ----------------------------------------------
  _constant_header fetchhl_,"FETCHHL"

  fetch.hl __

\ ----------------------------------------------
  _colon_header next_comma_,"NEXT,"

\ doc{
\
\ next,  ( -- )
\ 
\ Compile a Z80 jump to `next`.
\
\ }doc

  lit_ __ 0xE9DD __ \ opcode `jp (ix)`
  comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header pushhl_comma_,"PUSHHL,"

\ doc{
\
\ pushhl,  ( -- )
\ 
\ Compile a Z80 jump to `pushhl`.
\
\ }doc

  0xC3 _literal \ opcode `jp`
  c_comma_ __
  lit_ __ push_hl __ comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header pushhlde_comma_,"PUSHHLDE,"

\ doc{
\
\ pushhlde,  ( -- )
\ 
\ Compile a Z80 jump to `pushhlde`.
\
\ }doc

  0xC3 _literal \ opcode `jp`
  c_comma_ __
  lit_ __ push_hlde __ comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header fetchhl_comma_,"FETCHHL,"

\ doc{
\
\ fetchhl,  ( -- )
\ 
\ Compile a Z80 jump to `fetchhl`.
\
\ }doc

  0xC3 _literal \ opcode `jp`
  c_comma_ __
  lit_ __ fetch.hl __ comma_ __
  semicolon_s_ __

fetchhl_comma_nfa constant latest_nfa_in_assembler_voc

\ ----------------------------------------------
\ Start compiling in the `forth` vocabulary

nfa_of_the_previous_word defl 0 \ link to previous Forth word
\ current_vocabulary defl forth_pfa


\ ----------------------------------------------
  _colon_header label_,"LABEL"

  create_ __ asm_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header also_,"ALSO"

\ doc{
\
\ also  ( -- )
\
\ Duplicate the vocabulary at the top of the search order.
\
\ }doc

\ [Code adapted from F83.]

\ : also  ( -- )
\   context dup cell+ [ #vocs 2- cells ] literal cmove>  ;

  context_ __ dup_ __ cell_plus_ __
  lit_ __ max_search_order cell - cells __
  cmove_up_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header minus_order_,"-ORDER"

\ : -order  ( -- )  context [ #vocs cells ] literal erase  ;

  context_ __ lit_ __ max_search_order cells __ erase_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header only_,"ONLY"

\ doc{
\
\ only  ( -- )
\
\ Erase the search order and forces the `root` vocabulary to
\ be the first and second.
\
\ }doc

\ [Code adapted from F83.]

\ : only  ( -- )  -order root also  ;

  minus_order_ __
  root_ __
  also_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header previous_,"PREVIOUS"

\ doc{
\
\ previous  ( -- )
\
\ Remove the most recently referenced vocabulary from the search
\ order.
\
\ }doc

\ [Code adapted from F83.]

\ : previous  ( -- )
\   context dup cell+ swap [ #vocs 2- cells dup ] literal cmove
\   context literal + off  ;

  context_ __ dup_ __ cell_plus_ __ swap_ __
  lit_ __ max_search_order cell - cells __ cmove_ __
  context_ __ lit_ __ max_search_order cell - cells __ plus_ __ off_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header seal_,"SEAL"

\ doc{
\
\ seal  ( -- )
\
\ Change the search order such that only the vocabulary at the
\ top of the search order will be searched.
\
\ }doc

\ [Code adapted from F83.]

\ : seal  ( -- )  context @ -order context !  ;

  context_ __ fetch_ __ minus_order_ __ context_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------

  _does_header root_,"ROOT",,do_vocabulary

  latest_nfa_in_root_voc __

$ constant root_vocabulary_link
  0x0000 __

\ ----------------------------------------------

  _does_header forth_,"FORTH",,do_vocabulary

  latest_nfa_in_forth_voc __ \ nfa of the latest word defined in this vocabulary

$ constant forth_vocabulary_link
  root_vocabulary_link __

\ ----------------------------------------------
  _does_header assembler_,"ASSEMBLER",,do_vocabulary

\ XXX TODO move `assembler` and everthing related
\ to the library disk?

  latest_nfa_in_assembler_voc __
$ constant assembler_vocabulary_link
  forth_vocabulary_link __

\ ----------------------------------------------
  _colon_header s_lit_,"SLIT"

  \ : slit  ( -- ca len )  r@ count dup 1+ r> + >r  ;

  r_fetch_ __ count_ __ dup_ __ one_plus_ __
  from_r_ __ plus_ __ to_r_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_s_,"(S)"

\ doc{
\
\ (s) ( compilation: c "text<c>" -- ) ( run-time:  -- ca len )
\
\ }doc

  parse_ __ \ ( ca len )
  comp_question_ __
  zero_branch_ __ paren_s.interpreting __
  \ compiling
  s_literal_ __
  semicolon_s_ __ \ XXX TODO exit_
$ constant paren_s.interpreting
  save_string_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header c_lit_,"CLIT"

  ld a,(bc)
  bc incp
  \ XXX TODO include these entry points in the `assembler` vocabulary?
$ constant push_a
  a l ld
push_l: \ XXX TMP -- not used yet
  ld h,0
  jp push_hl

\ ----------------------------------------------
  _code_header lit_,"LIT"

  \ XXX FIXME -- crash if not compiling
  \ XXX TODO -- implement compile-only flag?

  ld a,(bc)
  bc incp
  a l ld
  ld a,(bc)
  bc incp
  a h ld
  jp push_hl

\ ----------------------------------------------
  _code_header bank_,"BANK"

\ doc{
\
\ bank  ( n -- )
\
\ Page memory bank _n_ (0..7) at 0xC000..0xFFFF.
\
\ }doc

  de pop \ e = bank
  bank.e call
  jpnext

$ constant bank.default
  \ XXX TODO ?
$ constant bank.names
  \ XXX TODO ?
$ constant bank.e
  \ ret \ XXX TMP for debugging
  ld a,(sys_bankm) \ get the saved status of BANKM
  and 0xF8 \ erase bits 0-2
  e or \ modify bits 0-2
  di
  ld (sys_bankm),a \ update BANKM
  out (bank1_port),a \ page the bank
  ei
  ret

\ ----------------------------------------------
  _code_header unused_csb_,"UNUSED-CSB"

\ doc{
\
\ csb-unused  ( -- len )
\
\ }doc

  ld hl,(unused_csb)
  jp push_hl

\ ----------------------------------------------
  _constant_header csb0_,"CSB0"

\ doc{
\
\ csb0  ( -- a )
\
\ }doc

  csb0 __

\ ----------------------------------------------
  _colon_header question_csb_,"?CSB"

\ doc{
\
\ ?csb  ( len -- )
\
\ Make sure there's room for the given characters.
\
\ }doc

  dup_ __ lit_ __ unused_csb __ fetch_ __ greater_than_ __
  zero_branch_ __ question_csb_.enough __
  \ not enough space; reset the pointer
  csb_size _literal
  lit_ __ unused_csb __ store_ __

$ constant question_csb_.enough
  negate_ __ lit_ __ unused_csb __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header allocate_string_,"ALLOCATE-STRING"

\ doc{
\
\ string-allocate  ( len -- ca )
\
\ }doc

  question_csb_ __
  csb0_ __ unused_csb_ __ plus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header save_string_,"SAVE-STRING"

\ doc{
\
\ save-string  ( ca1 len1 -- ca2 len1 )
\
\ }doc

  dup_ __ allocate_string_ __ swap_ __
  two_dup_ __ two_to_r_ __
  move_ __ two_from_r_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header save_counted_string_,"SAVE-COUNTED-STRING"

\ doc{
\
\ save-counted-string  ( ca1 len1 -- ca2 )
\
\ }doc

\ dup 1+ string-allocate dup >r $! r>

  dup_ __ one_plus_ __ allocate_string_ __
  dup_ __ to_r_ __ dollar_store_ __ from_r_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header empty_csb_,"EMPTY-CSB"

\ doc{
\
\ empty-csb  ( -- )
\
\ }doc

  lit_ __ csb __
  csb_total_size _literal
  erase_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header execute_,"EXECUTE"

\ doc{
\
\ execute  ( cfa  -- )
\
\ }doc

  hl pop
  jp next2

\ ----------------------------------------------
  _code_header perform_,"PERFORM"

\ doc{
\
\ perform  ( a  -- )
\
\ Execute the word whose cfa is stored in _a_.  Do nothing if
\ the content of _a_ is zero.
\
\ }doc

  hl pop
  ld a,(hl)
  hl incp
  ld h,(hl)
  a l ld
  h or
  jp nz,next2
  jpnext

\ ----------------------------------------------
  _colon_header forward_mark_,">MARK"

\ doc{
\
\ >mark  ( -- orig )  \ Forth-83, C, "forward-mark"
\
\ Compile space in the dictionary for a branch address which
\ will later be resolved by `>resolve`.
\
\ Used at the source of a forward branch.  Typically used after
\ either `branch`, `0branch` or `?branch`.
\
\ }doc

  question_comp_ __
  here_ __ zero_ __ comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header forward_resolve_,">RESOLVE"

\ doc{
\
\ >resolve  ( orig -- )  \ Forth-83, C, "forward-resolve"
\
\ Resolve a forward branch by placing the address of the current
\ dictionary pointer into the space compiled by `>mark`.
\
\ }doc

  question_comp_ __
  here_ __ swap_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header backward_mark_,"<MARK"

\ doc{
\
\ <mark  ( -- dest )  \ Forth-83, C, "backward-mark"
\
\ Leave the address of the current dictionary pointer, as the
\ the destination of a backward branch.  _dest_ is typically
\ only used by `<resolve` to compile a branch address.
\
\ }doc

  question_comp_ __
  here_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header backward_resolve_,"<RESOLVE"

\ doc{
\
\ <resolve  ( dest -- )  \ Forth-83, C, "backward-resolve"
\
\ Resolve a backward branch.  Compile a branch address using
\ _dest_, the address left by `<mark`,  as the destination
\ address.  Used at the source of a backward branch after either
\ `branch` or `?branch` or `0branch`.
\
\ }doc

  question_comp_ __
  comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header branch_,"BRANCH"

\ doc{
\
\ branch  ( -- )  \ ANS Forth
\
\ The run-time procedure to branch unconditionally. An in-line
\ offset is copied to the interpretive pointer IP to branch
\ forward or backward.
\
\ }doc

  b h ld
  c l ld \ hl = Forth IP, containing the address to jump to
  ld c,(hl)
  hl incp
  ld b,(hl) \ bc = New Forth IP
  jpnext

\ ----------------------------------------------
  _code_header zero_branch_,"0BRANCH"

\ doc{
\
\ 0branch  ( f -- )  \ fig-Forth
\
\ A run-time procedure to branch conditionally. If  _f_ on stack
\ is false (zero), the following in-line address is copied to IP
\ to branch forward or  backward.
\
\ }doc

  hl pop
  l a ld
  h or
  jp z,branch_pfa \ branch if zero
  bc incp
  bc incp \ skip the inline branch address
  jpnext

\ ----------------------------------------------
  _code_header question_branch_,"?BRANCH"

\ doc{
\
\ ?branch  ( f -- )
\
\ A run-time procedure to branch conditionally. If  _f_ on stack
\ is not zero, the following in-line address is copied to IP to
\ branch forward or backward.
\
\ Note: This is not Forth-83's `?branch`: Forth-83's `?branch`
\ does the same than fig-Forth's `0branch`: the branch is done
\ when the flag is zero. Solo Forth includes fig-Forth's
\ `0branch` and also `?branch`, that branches when the flag is
\ not zero.
\
\ }doc

  hl pop
  l a ld
  h or
  jp nz,branch_pfa \ branch if not zero
  bc incp
  bc incp \ skip the inline branch address
  jpnext

\ ----------------------------------------------
  _code_header paren_loop_,"(LOOP)"

  \ XXX NOTE:
  ;
  \ This code is from Abersoft Fort.  It's the same code used in
  \ `(+loop)` in fig-Forth 1.1g.  The author of Abersoft Forth
  \ used it to write `(loop)` and wrote `(+loop)` with a simple
  \ call to it, what saves code.
  ;
  \ XXX TODO -- The `(loop)` of DZX-Forth is much faster, but
  \ requires and additional parameter on the return stack.

  ld de,0x0001
$ constant paren_loop.step_in_de
  ld hl,(return_stack_pointer)
  ld a,(hl)
  add a,e
  ld (hl),a
  a e ld
  hl incp
  ld a,(hl)
  adc a,d
  ld (hl),a
  hl incp \ (hl) = limit
  d inc
  d dec
  a d ld \ de = new index
  jp m,paren_loop.negative_step

  \ increment>0
  e a ld
  sub (hl)
  d a ld
  hl incp
  sbc a,(hl)
  jp paren_loop.end

$ constant paren_loop.negative_step
  \ increment<0
  ld a,(hl) \ limit-index
  sub e
  hl incp
  ld a,(hl)
  sbc a,d \ a<0?

$ constant paren_loop.end
  jp m,branch_pfa \ loop again if a<0
  \ done, discard loop parameters
  hl incp
  ld (return_stack_pointer),hl
  \ skip branch offset
  bc incp
  bc incp
  jpnext

\ ----------------------------------------------
  _code_header paren_plus_loop_,"(+LOOP)"

  de pop
  jp paren_loop.step_in_de

\ ----------------------------------------------

0 [if]

  _colon_header paren_question_do_,"(?DO)"

  \ XXX TODO -- first draft, just copied from DZX-Forth

  \ XXX FIXME -- crash in both cases

  two_dup_ __ equals_ __
  question_branch_ __ paren_question.end __
  paren_do_ __
  semicolon_s_ __ \ XXX TODO exit_
$ constant paren_question.end
  two_drop_ __
  from_r_ __ fetch_ __ to_r_ __
  semicolon_s_ __

[then]

\ ----------------------------------------------
  _code_header paren_do_,"(DO)"

  \ [Code from CP/M fig-Forth 1.1g.]

  exx                           \ 04t 01b
  de pop                        \ 10t 01b
  bc pop                        \ 10t 01b
  ld hl,(return_stack_pointer)  \ 20t 03b
  hl decp                        \ 06t 01b
  ld (hl),b                     \ 07t 01b
  hl decp                        \ 06t 01b
  ld (hl),c                     \ 07t 01b
  hl decp                        \ 06t 01b
  ld (hl),d                     \ 07t 01b
  hl decp                        \ 06t 01b
  ld (hl),e                     \ 07t 01b
  ld (return_stack_pointer),hl  \ 16t 03b
  exx                           \ 04t 01b
                                ;116t 18b TOTAL
  jpnext

\ ----------------------------------------------
\ XXX TODO experimental do-loop structures adapted from Spectrum Forth-83
\ and F83.

\ A do-loop pushes three items on the return stack:
\
\ 0) Limit
\ 1) Reverse branch address (jump to here if loop repeats).
\ 2) Current index represented as `(index-limit) xor 0x8000`.
\    This is at the top.
\    The current index is represented this way so it is easier
\    to check whether index has crossed the boundary between
\    limit-1 and limit, acoording to
\    the rules of Forth-83, even with negative increment in +LOOP.

  _code_header paren_do83_,"(DO83)"

  hl pop \ initial constant
  de pop \ limit
$ constant paren_do83.de_hl
  \ de = limit
  \ hl = initial constant
  hl push \ initial constant  ( initial )
  ld hl,(return_stack_pointer)
  hl decp
  ld (hl),d
  hl decp
  ld (hl),e \ push limit constant on return stack ( R: initial )
  bc incp
  bc incp \ increment the Forth IP, skip branch address
  hl decp
  ld (hl),b
  hl decp
  ld (hl),c \ push current instruction pointer on return stack
  ex (sp),hl \ initial constant now in HL, return stack pointer on stack
  a and \ reset the carry flag
  sbc hl,de \ B SBCP        \ Subtract limit value.
  h a ld \ H A LD
  xor 0x80 \ 80 XOR#       \ Flip most significant bit.
  a d ld \ A B LD
  l e ld \ L C LD        \ Move result to DE.
  hl pop \ H POP        \ Get return stack pointer from stack,
  hl decp \ H DEC
  ld (hl),d \ B M LD
  hl decp \ H DEC
  ld (hl),e \ C M LD        \ Push (initial - limit) XOR 0x8000 onto return stack.
  ld (return_stack_pointer),hl \ RPTR STHL       \ Save return stack pointer.
  jpnext \ JPIX ;C

  _code_header paren_question_do83_,"(?DO83)"

  hl pop \ initial constant
  de pop \ limit
  a and \ reset the carry flag
  sbc hl,de \ compare
  jr z,question_do.equals
  \ not equals
  \ XXX TODO move add after sbc and save one jump
  add hl,de \ reverse the subtraction
  jp paren_do83.de_hl \ perform regular `do`
$ constant question_do.equals
  jp branch_pfa \ XXX TODO ?

  _colon_header question_do83_,"?DO83",immediate

  compile_ __ paren_question_do83_ __
  forward_mark_ __
fig_compiler_security? [if]
  3 _literal
[then]
  semicolon_s_ __

  _colon_header do83_,"DO83",immediate

  compile_ __ paren_do83_ __
  forward_mark_ __
fig_compiler_security? [if]
  3 _literal
[then]
  semicolon_s_ __

  _colon_header loop83_,"LOOP83",immediate

fig_compiler_security? [if]
  3 _literal
  question_pairs_ __
[then]
  compile_ __ paren_loop83_ __
  forward_resolve_ __
  semicolon_s_ __

  \ _colon_header plus_loop83_,"+LOOP83",immediate

  \ c_lit_ __
  \ 3 _
  \ question_pairs_ __
  \ compile_ __ paren_plus_loop83_ __
  \ forward_resolve_ __
  \ semicolon_s_ __

  _colon_header paren_loop83_,"(LOOP83)"

  ld hl,(return_stack_pointer)
  ld e,(hl) \ M C LD
  hl incp    \ H INC
  ld d,(hl) \ M B LD         \ Read current index value.
  de incp    \ B INC         \ Increment it.
  d a ld    \ B A LD
  xor 0x80  \ 80 XOR#
  e or      \  C OR         \ Was it equal to 0x8000 ?
  jp nz,paren_loop83.loop \ jump if not

  \ The real index has reached limit, terminate loop.
  \ Increment ret stack pointer by 5 (1 increment already done).
  ld de,5   \ 5 B LDP#
  add hl,de \ B ADDP
  ld (return_stack_pointer),hl \ RPTR STHL
  jpnext

$ constant paren_loop83.loop

  ld (hl),d \  B M LD
  hl decp    \ H DEC
  ld (hl),e \ C M LD    \ Store updated index.
  hl incp    \ H INC
  hl incp    \ H INC
  ld c,(hl) \ M E LD
  hl incp    \ H INC
  ld c,(hl) \ M D LD   \ Read loop start address into instruction pointer, repeat loop.
  jpnext

0 [if] \ XXX TODO adapt

\ CODE (+LOOP83) ( w --- )
\  RPTR LDHL     \ Read return stack pointer into HL.
\    M C LD
\     H INC
\    M B LD      \ Read Current index.
\      EXSP      \ HL now contains __ the increment value.
\     A AND
\    B ADCP      \ Add increment to index.
\    v if
\                \ If overflow, then boundary between limit-1 and limit is
\                \ crossed, terminate loop.
\      H POP     \ Get return stack pointer.
\   5 B LDP#
\     B ADDP
\  RPTR STHL     \ Increment ret stack pointer by 5 (1 increment already done)
\                \ and store updated ret stack pointer back.
\    else
\     H B LD
\     L C LD     \ Move updated index to BC.
\      H POP     \ Get return stack pointer.
\     B M LD
\      H DEC
\     C M LD     \ Store updated index.
\      H INC
\      H INC
\     M E LD
\      H INC
\     M D LD     \ Read loop start address into instruction pointer, repeat loop.
\    then
\  JPIX ;C

\ CODE LEAVE83
\  RPTR LDHL     \ Read return stack pointer into HL.
\     H INC
\     H INC
\    M E LD
\     H INC
\    M D LD      \ Get start address into DE.
\     H INC
\     H INC
\     H INC
\ RPTR STHL      \ Write updated return stack pointer (6 was added).
\     D DEC
\     D DEC      \ DE (instruction pointer) now points to forward branch address
\   'BRANCH @ JP ;C \ continue into BRANCH.

\ CODE I83  ( --- w)
\ RPTR LDHL        \ Read return stack pointer into HL.
\   \ J jumps here.
\   M C LD
\    H INC
\   M B LD         \ Read current index. (which is (index-limit) xor 0x8000.
\    H INC
\    H INC
\    H INC
\   M A LD         \ Read limit and add to index
\    C ADD
\   A C LD
\    H INC
\   M A LD
\    B ADC
\  80 XOR#         \ and flip most significant bit, getting true index value.
\   A B LD
\   B PUSH         \ Push result.
\ JPIX ;C

\ CODE J83 ( --- w)
\   RPTR LDHL      \ Read return stack pointer into HL
\   6 B LDP#
\     B ADDP       \ Add 6 to it, to get to next inner loop parameters.
\  'I @ 3 + JR ;C  \ Continue into I.

\ CODE I'83 ( --- w)
\   RPTR LDHL      \ Read return stack pointer into HL
\      H INC
\      H INC
\      H INC
\      H INC
\     M C LD
\      H INC
\     M B LD       \ Read limit value.
\     B PUSH       \ Push result
\     JPIX ;C

[then]

\ ----------------------------------------------
  _code_alias_header i_,"I",,r_fetch_

\ doc{
\
\ i  ( -- x ) ( R: loop-sys -- loop-sys ) \ ANS Forth
\
\ Return a copy of the current (innermost) loop index.
\
\ }doc

\ ----------------------------------------------
  _code_header digit_,"DIGIT"

\ doc{
\
\ digit  ( c n1 --- n2 tf | ff )  \ fig-Forth
\
\ Convert the ascii character _c_ (using base _n1_) to its
\ binary equivalent n2, accompanied by a true flag. If the
\ conversion is invalid, leave only a false flag.
\
\ }doc

  hl pop  \ l=base
  de pop  \ e=character
  e a ld  \ character
  sub '0' \ >="0"
  jp c,false_pfa \ <"0" is invalid
  cp 0x0A \ >"9"?
  jp m,digit.test_value \ no, test constant
  sub 0x07 \ gap between "9" & "A", now "A"=0x0A
  cp 0x0A \ >="A"?
  jp c,false_pfa \ characters between "9" & "A" are invalid
$ constant digit.test_value
  cp l \ <base?
  jp nc,false_pfa \ no, invalid
  a e ld \ converted digit
  de push
  jp true_pfa

\ ----------------------------------------------
  _code_header paren_find_,"(FIND)"

\ doc{
\
\ (find)  ( ca nfa --- ca 0 | cfa 1 | cfa -1 )
\
\ Find the definition named in the counted string at _ca_,
\ starting at _nfa_. If the definition is not found, return _ca_
\ and zero.  If the definition is found, return its _cfa_. If
\ the definition is immediate, also return one (1); otherwise
\ also return minus-one (-1).
\
\ }doc

  ld e,names_bank
  bank.e call \ page the memory bank

  de pop \ nfa
  hl pop \ string address
  bc push \ save the Forth IP
  ld (paren_find.string_address),hl

  \ XXX FIXME the string searched for must be in the string
  \ buffer, below 0xC000! This is not a problem now, during the
  \ development, because the dictionary is small.

$ constant paren_find.begin
  \ _z80_border 2 \ XXX INFORMER
  \ _z80_border 7 \ XXX INFORMER
  \ Compare the string with a new word.
  \ de = nfa
  ld (paren_find.nfa_backup),de \ save the nfa for later
paren_find.string_address: equ $+1
  ld hl,0 \ string address
  ld a,(de) \ length byte of the name field
  a c ld    \ save for later
  and max_word_length_bit_mask  \ length
  cp (hl) \ same length?
  jr nz,paren_find.not_a_match \ lengths differ

  \ Lengths match, compare the characters.
  a b ld \ length
$ constant paren_find.compare_next_char
  hl incp \ next character in string
  de incp \ next character in name field
  ld a,(de)
  cp (hl) \ match?
  jr nz,paren_find.not_a_match \ no match
  djnz paren_find.compare_next_char \ match so far, loop again

  \ The string matches.
  \ c = name field length byte
  ld hl,(paren_find.nfa_backup)
\  ld (0xfffa),hl \ XXX INFORMER ; nfa, ok
  hl decp
  hl decp \ lfa
  hl decp \ high part of the pointer to cfa
  ld d,(hl)
  hl decp \ low part of the pointer to cfa
  ld e,(hl) \ de = cfa

\  ld (0xfffc),de \ XXX INFORMER ; cfa, ok

  ld hl,1 \ 1=immediate word
  c a ld \ name field length byte
  and precedence_bit_mask \ immediate word?
  jp nz, paren_find.end
  \ non-immediate word
  hl decp
  hl decp \ -1 = non-immediate word

$ constant paren_find.end
  \ If match found:
  \   de = cfa
  \   hl = -1 | 1
  \ If no match found:
  \   de = ca
  \   hl = 0
  exx
  ld e,default_bank
  bank.e call \ page the default memory bank
  exx
  bc pop \ restore the Forth IP
  \ _z80_border 4 \ XXX INFORMER
  jp push_hlde

$ constant paren_find.not_a_match
  \ Not a match, try next one.
paren_find.nfa_backup: equ $+1
  ld hl,0 \ nfa
  hl decp \ high address of lfa
  ld d,(hl) \ high part of the next nfa
  hl decp \ low address of lfa
  ld e,(hl) \ low part of the next nfa
  d a ld
  e or \ end of dictionary? (next nfa=0)
  jp nz,paren_find.begin \ if not, continue
  \ End of dictionary, no match found, return.
  ld de,(paren_find.string_address)
  ld hl,0
  jp paren_find.end

\ ----------------------------------------------
  _code_header scan_,"SCAN"

\ doc{
\
\ scan  ( ca c -- ca len )
\
\ c = ascii delimiting character
\ ca = text address
\ len = length of the parsed text
\
\ }doc

  hl pop \ delimiter
  de pop \ address
  de push
  bc push \ save Forth IP
  l c ld \ delimiter
  ld hl,0 \ length
  hl decp
  de decp
$ constant scan.begin
  hl incp
  de incp
  ld a,(de)
  cp c \ delimiter?
  jr nz,scan.begin
  \ delimiter found
  bc pop \ restore Forth IP
  jp push_hl

\ ----------------------------------------------
  _code_header skip_,"SKIP"

\ doc{
\
\ skip  ( ca1 c -- ca2 )
\
\ }doc

  de pop \ e = delimiter
  hl pop \ ca1
$ constant skip.begin
  ld a,(hl)
  cp e \ delimiter?
  jp nz,push_hl
  hl incp
  jp skip.begin \ again

\ ----------------------------------------------
  _code_header chan_,"CHAN"

\ doc{
\
\ chan  ( n -- )  \ Open channel n for output.
\
\ }doc
\
\ [Code from Spectrum Forth-83.]

  hl pop
  bc push
  l a ld
  rom_chan_open call
  bc pop
  jpnext

\ ----------------------------------------------
  _colon_header emit_,"EMIT"

\ XXX TODO -- Add multitasker's `pause` when available.
\ XXX TODO -- defer

  paren_emit_ __
  one_ __ out_ __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header paren_emit_,"(EMIT)"

\ doc{
\
\ (emit)  ( b -- )
\
\ Send the character b to the current channel.
\
\ }doc

\ [Code from Spectrum Forth-83's `TOCH`.]

latin1_charset_in_bank? [if]
  ld e,names_bank
  bank.e call \ the charset is in the memory bank
[then]
  hl pop
  l a ld
  ld (iy+sys_scr_ct_offset),0xFF \ no scroll message
  rst 0x10
latin1_charset_in_bank? [if]
  ld e,default_bank
  bank.e call
[then]
  jpnext

\ ----------------------------------------------
  _colon_header printer_,"PRINTER"

  3 _literal
  chan_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header display_,"DISPLAY"

  two_ __
  chan_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header key_question_,"KEY?"

\ doc{
\
\ key?  ( -- f )  \ ANS Forth
\
\ }doc

  ld a,(sys_last_k)
  a and
  jp z,false_pfa
  jp true_pfa

\ ----------------------------------------------
  _variable_header decode_table_,"DECODE-TABLE"

  0 __ \ no chained table ; XXX TODO
              \ Symbol Shift + Letter --> new char
  0xC6 _ '[' _ \ "Y" 198 (0xC6) "AND"  --> 091 (0x5B) "["
  0xC5 _ ']' _ \ "U" 197 (0xC5) "OR"   --> 093 (0x5D) "]"
  0xE2 _ '~' _ \ "A" 226 (0xE2) "STOP" --> 126 (0x7E) "~"
  0xC3 _ '|' _ \ "S" 195 (0xC3) "NOT"  --> 124 (0x7C) "|"
  0xCD _ '\' _ \ "D" 205 (0xCD) "STEP" --> 092 (0x5C) "\"
  0xCC _ '{' _ \ "F" 204 (0xCC) "TO"   --> 123 (0x7B) "{"
  0xCB _ '}' _ \ "G" 203 (0xCB) "THEN" --> 125 (0x7D) "}"
  0 _ \ end of data

  \ "I" 172 (0xAC) "AT"   --> 127 (0x7F) "(C)" \ XXX TODO

\ ----------------------------------------------
  _code_header decode_char_,"DECODE-CHAR"
  
\ doc{
\
\ decode-char  ( c1 -- c2 )
\
\ }doc

  \ XXX TODO

  de pop
  ld hl,decode_table_+2 \ XXX TMP
$ constant decode_char.begin
  ld a,(hl)
  a and
  e a ld

\ ----------------------------------------------
  _code_header paren_key_,"(KEY)"

  ld a,(sys_last_k)
  ld (previous_key),a
$ constant paren_key_.begin
  \ pause call \ XXX TODO
  \ rom_keyboard call \ XXX TODO not needed if system interrupts are on
  ld a,(sys_last_k)
previous_key: equ $+1
  cp 0 \ a different key?
  jp z,paren_key_.begin
  ld h,0
  a l ld
  a xor
  ld (sys_last_k),a \ delete the last key
  hl push
  jp decode_char_

\ ----------------------------------------------
  _code_header key_,"KEY"

\ doc{
\
\ key  ( -- c )  \ ANS Forth
\
\ }doc

\ XXX -- This version works also when the system interrupts are off.
\ XXX TODO -- Add multitasker's `pause` when available.

  bc push
  push ix \ XXX TMP
$ constant key.begin
  rom_key_scan call
  jr nz,key.begin
  rom_key_test call
  jr nc,key.begin
  d dec
  a e ld
  rom_key_decode call
$ constant key.end
  ld hl,sys_last_k
  ld (hl),0
  pop ix \ XXX TMP
  bc pop
  jp push_a

\ ----------------------------------------------
  _code_header xkey_,"XKEY"

\ XXX OLD -- this is the original code from Abersoft Forth.
\ Too complex. A mode-less version will be coded.

\ doc{
\
\ xkey  ( -- c )
\
\ Leave the ASCII constant of the next terminal key struck.
\
\ }doc

  \ XXX TODO simplify, no Spectrum modes

  bc push

  \ XXX OLD
  \ inverse video on
  \ ld a,inverse_char
  \ rst 0x10
  \ ld a,0x01
  \ rst 0x10

$ constant xkey.new_key
  a xor
  ld (sys_last_k),a

  \ Print cursor:
  ld a,0x88 \ cursor
  rst 0x10
  ld a,backspace_char
  rst 0x10

$ constant xkey.wait_for_key
  ld a,(sys_last_k)
  a and
  jr z,xkey.wait_for_key
  \ a = pressed key code

  cp caps_char \ toggle caps lock?
  jr nz,xkey.translate
  \ toggle caps lock
  ld hl,sys_flags2
  ld a,0x08
  (hl) xor
  ld (hl),a
  jr xkey.new_key

  \ Translate some chars
  \ XXX TODO use a configurable list of chars pairs

$ constant xkey.translate
$ constant xkey.left_bracket
  cp 0xC6
  jr nz,xkey.right_bracket
  ld a,'['
$ constant xkey.right_bracket
  cp 0xC5
  jr nz,xkey.tilde
  ld a,']'
$ constant xkey.tilde
  cp 0xE2
  jr nz,xkey.vertical_bar
  ld a,'~'
$ constant xkey.vertical_bar
  cp 0xC3
  jr nz,xkey.backslash
  ld a,'|'
$ constant xkey.backslash
  cp 0xCD
  jr nz,xkey.left_curly_bracket
  ld a,'\'
$ constant xkey.left_curly_bracket
  cp 0xCC
  jr nz,xkey.right_curly_bracket
  ld a,'{'
$ constant xkey.right_curly_bracket
  cp 0xCB
  jr nz,xkey.end
  ld a,'}'

$ constant xkey.end
  a l ld
  ld h,0x00

  \ XXX OLD
  \ inverse video off
  \ ld a,inverse_char
  \ rst 0x10
  \ ld a,0x00
  \ rst 0x10

  \ delete the cursor
  ld a,space_char
  rst 0x10
  ld a,backspace_char
  rst 0x10

  bc pop
  jp push_hl

\ ----------------------------------------------
  _colon_header cr_,"CR"

\ doc{
\
\ cr  ( -- )
\
\ Transmit a carriage return to the selected output device.
\
\ }doc

  carriage_return_char _literal
  emit_ __ out_ __ off_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header cmove_up_,"CMOVE>"

  exx
  bc pop
  de pop
  hl pop
  move_block_downwards call
  exx
  jpnext

\ ----------------------------------------------
  _code_header cmove_,"CMOVE"

  exx
  bc pop
  de pop
  hl pop
  move_block_upwards call
  exx
  jpnext

\ ----------------------------------------------
  _code_header move_,"MOVE"

\ doc{
\
\ move  ( a1 a2 len -- )
\
\ }doc

  exx
  bc pop
  de pop
$ constant move.do
  hl pop
  move_block call
  exx
  jpnext

\ ----------------------------------------------
  _code_header smove_,"SMOVE"

\ doc{
\
\ smove  ( a1 len a2 -- )
\ 
\ Move the string _a1 len_ to _a2_. _a2_ will contain the first
\ char of the string.
\
\ }doc

\ swap move

  exx
  de pop
  bc pop
  jp move.do

\ ----------------------------------------------
  _code_header u_m_star_,"UM*"

\ doc{
\
\ um*  ( u1 u2 -- ud )  \ ANS Forth
\
\ Multiply _u1_ by _u2_, giving the unsigned double-cell product
\ _ud_.  All values and arithmetic are unsigned.
\
\ }doc

  \ [Code from DZX-Forth.]

  de pop
  hl pop
  hl_multiplied_by_de_to_hlde_unsigned call
  jp push_hlde

\ ----------------------------------------------
  _code_header u_slash_mod_,'U/MOD' \ XXX OLD

\ doc{
\
\ u/mod ( ud u1 -- u2 u3 )
\
\ Divide _ud_ by _u1_, giving the quotient _u3_ and the
\ remainder _u2_.  All values and arithmetic are unsigned. An
\ ambiguous condition exists if u1 is zero or if the quotient
\ lies outside the range of a single-cell unsigned integer.
\
\ }doc

\ XXX FIXME -- This word, whose code is taken from Abersoft
\ forth, has a bug that affects `(line)`, used by `message`, and
\ other words that use it: `*/mod`, `mod` and `/mod`, with
\ certain negative values, return different values in Abersoft
\ Forth and other Forth systems that have been tested (some of
\ them are fig-Forth).

  ld hl,0x0004
  add hl,sp
  ld e,(hl)
  ld (hl),c
  hl incp
  ld d,(hl)
  ld (hl),b
  bc pop
  hl pop
  l a ld
  sub c
  h a ld
  sbc a,b
  jr c,l60a0h
  ld hl,0xFFFF
  ld de,0xFFFF
  jr l60c0h
$ constant l60a0h
  ld a,0x10
$ constant l60a2h
  add hl,hl
  rla
  ex de,hl
  add hl,hl
  jr nc,l60aah
  de incp
  a and
$ constant l60aah
  ex de,hl
  rra
  af push
  jr nc,l60b4h
  l and
  sbc hl,bc
  jr l60bbh
$ constant l60b4h
  a and
  sbc hl,bc
  jr nc,l60bbh
  add hl,bc
  de decp
$ constant l60bbh
  de incp
  af pop
  a dec
  jr nz,l60a2h
$ constant l60c0h
  bc pop
  hl push
  de push
  jpnext

0 [if] \ XXX TODO

\ ----------------------------------------------
  _code_header s_m_slash_rem_,"SM/REM"

\ doc{
\
\ sm/rem  ( d1 n1 -- n2 n3 )  \ ANS Forth,  "s-m-slash-rem"
\
\ Symmetric division:
\
\   d1 = n3*n1+n2, sign(n2)=sign(d1) or 0.
\
\ Divide _d1_ by _n1_, giving the symmetric quotient _n3_ and
\ the remainder _n2_. Input and output stack arguments are
\ signed.
\
\ }doc

\ XXX TODO check: An ambiguous condition exists if n1 is zero or
\ if the quotient lies outside the range of a single-cell signed
\ integer.

\ [Code from DZX-Forth.]

  c l ld
  b h ld
  bc pop
  de pop
  ex (sp),hl
  ex de,hl
$ constant s_m_slash_rem_.1
  msm call
  jp msm.2

\ ----------------------------------------------
  _code_header fm_slash_mod_,"FM/MOD"

\ doc{
\
\ fm/mod  ( d1 n1 -- n2 n3 )  \ ANS Forth,  "f-m-slash-mod"
\
\ Floored division:
\
\   d1 = n3*n1+n2, n1>n2>=0 or 0>=n2>n1.
\
\ Divide _d1_ by _n1_, giving the floored quotient _n3_ and
\ the remainder _n2_. Input and output stack arguments are
\ signed.
\
\ }doc

\ XXX TODO check: An ambiguous condition exists if n1 is zero or
\ if the quotient lies outside the range of a single-cell signed
\ integer.

\ [Code from DZX-Forth.]

  c l ld
  b h ld
  bc pop
  de pop
  ex (sp),hl
  ex de,hl
$ constant fm_slash_mod.1
  msm call
  d a ld
  e or
  jp z,msm.2    \ skip if remainder = 0
  hl decp    \ floor
  hl push
  ex de,hl
  add hl,bc
  ex de,hl
  hl pop
  jp msm.2

[then]

\ ----------------------------------------------
  _code_header and_,"AND"

  de pop
  hl pop
  e a ld
  l and
  a l ld
  d a ld
  h and
  a h ld
  jp push_hl

\ ----------------------------------------------
  _code_header or_,"OR"

  de pop
  hl pop
  e a ld
  l or
  a l ld
  d a ld
  h or
  a h ld
  jp push_hl

\ ----------------------------------------------
  _code_header xor_,"XOR"

  de pop
  hl pop
  e a ld
  l xor
  a l ld
  d a ld
  h xor
  a h ld
  jp push_hl

\ ----------------------------------------------
  _constant_header np_,"NP"

  names_pointer __

\ ----------------------------------------------
  _constant_header np0_,"NP0"

  \ XXX OLD -- not used

  names_bank_address __

\ ----------------------------------------------
  _code_header np_fetch_,"NP@"

  ld hl,(names_pointer)
  jp push_hl

\ ----------------------------------------------
  _code_header np_store_,"NP!"

  hl pop
  ld (names_pointer),hl
  jpnext

\ ----------------------------------------------

  _colon_header comma_np_,",NP"

\ doc{
\
\ ,np  ( x -- )
\
\ Store _x_ into the next available names memory cell, advancing
\ the names pointer.
\
\ Note: The names memory is supposed to be paged in.
\
\ }doc

  np_fetch_ __ store_ __ two_ __ np_ __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header sp_fetch_,"SP@"

  ld hl,0x0000
  add hl,sp
  jp push_hl

\ ----------------------------------------------
  _code_header sp_store_,"SP!"

\ doc{
\
\ sp!  ( a -- )
\
\ Store _a_ into the stack pointer.
\
\ }doc

  hl pop
  ld sp,hl
  jpnext

\ ----------------------------------------------
  _constant_header rp_,"RP"

  return_stack_pointer __

\ ----------------------------------------------
  _code_header rp_fetch_,"RP@"

  ld hl,(return_stack_pointer)
  jp push_hl

\ ----------------------------------------------
  _code_header rp_store_,"RP!"

\ doc{
\
\ rp!  ( a -- )
\
\ Store _a_ into the return stack pointer.
\
\ }doc

0 [if] \ XXX OLD
  ld hl,(user_variables_pointer)
  hl incp
  hl incp \ hl=address of r0
  ld a,(hl)
  hl incp
  ld h,(hl)
  a l ld
[else]
  hl pop
[then]
  ld (return_stack_pointer),hl
  jpnext

\ ----------------------------------------------
1 [if] \ fig_exit?
  _code_header semicolon_s_,";S"
[else]
  _code_header exit_,"EXIT"
semicolon_s_ equ exit_
[then]


\ doc{
\
\ ;s  ( -- )  \ fig-Forth
\
\ Return execution to the calling definition.  Unnest one level.
\
\ It is used to stop interpretation of a screen. It is also the
\ run-time word compiled at the end of a colon-definition which
\ returns execution to the calling procedure.
\
\ }doc

\ XXX TODO combine this `;s` with `exit`?

  ld hl,(return_stack_pointer)
  ld c,(hl)
  hl incp
  ld b,(hl)
  hl incp
  ld (return_stack_pointer),hl
  jpnext

\ ----------------------------------------------
  _code_header pick_,"PICK"

  hl pop
  add hl,hl
  add hl,sp
  jp fetch.hl

\ ----------------------------------------------
  _code_alias_header unloop_,"UNLOOP",,two_r_drop_

\ doc{
\
\ unloop  ( -- ) ( R: x1 x2 -- )  \ ANS Forth, C
\
\ x1 = loop index
\ x2 = loop limit
\
\ Discard the loop-control parameters for the current nesting
\ level. An `unloop` is required for each nesting level before
\ the definition may be exited with `exit`. An ambiguous
\ condition exists if the loop-control parameters are
\ unavailable.
\
\ }doc

\ ----------------------------------------------
  _code_header exhaust_,"EXHAUST"

\ doc{
\
\ exhaust  ( -- ) ( R: n1 n2 -- n2 n2 )
\
\ n1 = loop limit
\ n2 = loop index
\
\ Force termination of a do-loop at the next opportunity by
\ setting the loop limit equal to the current constant of the
\ index. The index itself remains unchanged, and execution
\ proceeds normally until `loop` or `+loop` is encountered.
\
\ Note: This is the equivalent of fig-Forth's `leave`.
\
\ }doc

  ld hl,(return_stack_pointer)
  ld e,(hl)
  hl incp
  ld d,(hl)
  hl incp
  ld (hl),e
  hl incp
  ld (hl),d
  jpnext

\ ----------------------------------------------
  _code_header question_exhaust_,"?EXHAUST"

\ doc{
\
\ ?exhaust  ( f -- ) ( R: n1 n2 -- n1 n2 | n2 n2 )
\
\ n1 = loop limit
\ n2 = loop index
\ 
\ If _f_ is not false, force termination of a do-loop at the
\ next opportunity by setting the loop limit equal to the
\ current constant of the index. The index itself remains
\ unchanged, and execution proceeds normally until `loop` or
\ `+loop` is encountered.
\
\ }doc

  hl pop
  a h ld
  l or
  jp nz,exhaust_pfa
  jpnext

\ ----------------------------------------------
  _code_header to_r_,">R"

\ doc{
\
\ >r  ( x -- ) ( R: -- x )
\
\ }doc

  de pop
  ld hl,(return_stack_pointer)
  hl decp
  ld (hl),d
  hl decp
  ld (hl),e
  ld (return_stack_pointer),hl
  jpnext

\ ----------------------------------------------
  _code_header from_r_,"R>"

\ doc{
\
\ r>  ( -- x ) ( R: x -- )
\
\ }doc

  ld hl,(return_stack_pointer)
  ld e,(hl)
  hl incp
  ld d,(hl)
  hl incp
  ld (return_stack_pointer),hl
  de push
  jpnext

\ ----------------------------------------------
  _code_header two_r_drop_,"2RDROP"

\ doc{
\
\ 2rdrop  ( R: x1 x2 -- )
\
\ }doc

  ld hl,(return_stack_pointer)
  ld de,cell*2
  add hl,de
  ld (return_stack_pointer),hl
  jpnext

\ ----------------------------------------------
  _code_header r_drop_,"RDROP"

\ doc{
\
\ rdrop  ( R: x -- )
\
\ }doc

  ld hl,(return_stack_pointer)
  hl incp
  hl incp
  ld (return_stack_pointer),hl
  jpnext

\ ----------------------------------------------
  _code_header two_to_r_,"2>R"

\ doc{
\
\ 2>r  ( -- x1 x2 ) ( R: x1 x2 -- )
\
\ }doc

  ld hl,(return_stack_pointer)
  ld de,-cell*2
  add hl,de
  ld (return_stack_pointer),hl
  jp two_store.into_hl_pointer

\ ----------------------------------------------
  _code_header two_from_r_,"2R>"

\ 2r>  ( -- x1 x2 ) ( R: x1 x2 -- )

  ld hl,(return_stack_pointer)
  hl push
  ld de,cell*2
  add hl,de
  ld (return_stack_pointer),hl
  jp two_fetch_pfa

\ ----------------------------------------------
  _code_header two_r_fetch_,"2R@"

  ld hl,(return_stack_pointer)
  jp two_fetch.hl

\ ----------------------------------------------
  _code_header r_fetch_,"R@"

  ld hl,(return_stack_pointer)
  jp fetch.hl

\ ----------------------------------------------
  \ XXX FIXME as Error: confusion in formal parameters
  \ because of the string, why?
  _code_header zero_equals_,"0="

  hl pop
  l a ld
  h or
  jp z,true_pfa
  jp false_pfa

\ ----------------------------------------------
  _code_header zero_not_equals_,"0<>"

  hl pop
  l a ld
  h or
  jp z,false_pfa
  jp true_pfa

\ ----------------------------------------------
  _code_header zero_less_than_,"0<"

  hl pop
zero_less_.hl: \ XXX entry not used yet
  size_optimization? [if]
    add hl,hl \ 11t, 1 byte
  [else]
    \ [Idea from Ace Forth.]
    rl h \ 8t, 2 bytes
  [then]
$ constant true_if_cy
  jp c,true_pfa
  jp false_pfa

\ ----------------------------------------------
  _code_header zero_greater_than_,"0>"

  \ [Code from DZX-Forth.]

  de pop
  ld hl,0
  jp is_de_less_than_hl

\ ----------------------------------------------
  _code_header plus_,"+"

  de pop
  hl pop
  add hl,de
  jp push_hl

\ ----------------------------------------------
  _code_header d_plus_,"D+"

\ XXX TODO move to the library.

\ [Code from fig-Forth 1.1g.]
   
              \                           t  B
              \                           -- --
  exx         \ save ip                   04 01
  bc pop      \ (bc)<--d2h                10 01
  hl pop      \ (hl)<--d2l                10 01
  af pop      \ (af)<--d1h                10 01
  de pop      \ (de)<--d1l                10 01
  af push    \ (s1)<--d1h                11 01
  add hl,de   \ (hl)<--d2l+d1l=d3l        11 01
  ex  de,hl   \ (de)<--d3l                04 01
  hl pop      \ (hl)<--d1h                10 01
  adc hl,bc   \ (hl)<--d1h+d2h+carry=d3h  15 02
  de push    \ (s2)<--d3l                11 01
  hl push    \ (s1)<--d3h                11 01
  exx         \ restore ip                04 01
  jpnext    \                           08 02
              \                          --- --
              \                          134 15 TOTALS

\ ----------------------------------------------
  _code_header negate_,"NEGATE"

  de pop
  ld hl,0x0000
  a and
  sbc hl,de
  jp push_hl

\ ----------------------------------------------
  _code_header dnegate_,"DNEGATE"

  \ XXX TODO move to the disk

  hl pop
  de pop
  sub a
  sub e
  a e ld
  ld a,0x00
  sbc a,d
  a d ld
  ld a,0x00
  sbc a,l
  a l ld
  ld a,0x00
  sbc a,h
  a h ld
  jp push_hlde

\ ----------------------------------------------
  _code_header nip_,"NIP"

  hl pop
  de pop
  jp push_hl

\ ----------------------------------------------
  _code_header tuck_,"TUCK"

  hl pop
  de pop
  hl push
  jp push_hlde

\ ----------------------------------------------
  _code_header over_,"OVER"

  de pop
  hl pop
  hl push
  jp push_hlde

\ ----------------------------------------------
  _code_header drop_,"DROP"

  hl pop
  jpnext

\ ----------------------------------------------
  _code_header swap_,"SWAP"

  hl pop
  ex (sp),hl
  jp push_hl

\ ----------------------------------------------
  _code_header dup_,"DUP"

  hl pop
  hl push
  jp push_hl

\ ----------------------------------------------
  _code_header two_dup_,"2DUP"

  hl pop
  de pop
  de push
  hl push
  jp push_hlde

\ ----------------------------------------------
  _code_header plus_store_,"+!"

  hl pop \ variable address
  de pop \ number
  ld a,(hl)
  add a,e
  ld (hl),a
  hl incp
  ld a,(hl)
  adc a,d
  ld (hl),a
  jpnext

\ ----------------------------------------------
  _code_header off_,"OFF"

  hl pop
  ld (hl),0
  hl incp
  ld (hl),0
  jpnext

\ ----------------------------------------------
  _code_header on_,"ON"

  hl pop
true_flag 1 = [if]
  ld (hl),1
  hl incp
  ld (hl),0
[else]
  ld (hl),0xFF
  hl incp
  ld (hl),0xFF
[then]
  jpnext

\ ----------------------------------------------
  _code_header toggle_,"TOGGLE"

\ doc{
\
\ toggle  ( a b -- )  \ fig-Forth
\
\ Complement the contents of _a_ by the bit pattern _b_.
\
\ }doc

  de pop \ e = bit pattern
  hl pop \ address
  ld a,(hl)
  e xor
  ld (hl),a
  jpnext

\ ----------------------------------------------
  _code_header fetch_,"@"

  hl pop
$ constant fetch.hl
  ld e,(hl)
  hl incp
  ld d,(hl)
  de push
  jpnext

\ ----------------------------------------------
  _code_header c_fetch_,"C@"

  hl pop
  ld l,(hl)
  ld h,0x00
  jp push_hl

\ ----------------------------------------------
  _code_header two_fetch_,"2@"

  hl pop \ address
$ constant two_fetch.hl
  ld e,(hl)     \ 07t  1
  hl incp        \ 06t  1
  ld d,(hl)     \ 07t  1 ; de = low part
  hl incp        \ 06t  1
  ld a,(hl)     \ 07t  1
  hl incp        \ 06t  1
  ld h,(hl)     \ 07t  1
  a l ld        \ 04t  1 ; hl = high part
  ex de,hl      \ 04t  1
  jp push_hlde \ 10t  3
                \ 11t  0 de push
                \ 11t  0 hl push
                \ 86t 12 TOTAL

\ ----------------------------------------------
  _code_header two_store_,"2!"

  hl pop
$ constant two_store.into_hl_pointer
  de pop
  ld (hl),e
  hl incp
  ld (hl),d
  hl incp
  size_optimization? [if]
    jp store.into_hl_pointer
  [else]
    de pop
    ld (hl),e
    hl incp
    ld (hl),d
    jpnext
  [then]

\ ----------------------------------------------
  _code_header store_,"!"

  hl pop
$ constant store.into_hl_pointer
  de pop
$ constant store.de_into_hl_pointer
  ld (hl),e
  hl incp
  ld (hl),d
  jpnext

\ ----------------------------------------------
  _code_header c_store_,"C!"

  hl pop
  de pop
  ld (hl),e
  jpnext

\ ----------------------------------------------
  _colon_header colon_,":",immediate

  question_exec_ __
  store_csp_ __
  header_ __ right_bracket_ __
  paren_semicolon_code_ __
$ constant do_colon
  ld hl,(return_stack_pointer)
  hl decp
  ld (hl),b
  hl decp
  ld (hl),c
  ld (return_stack_pointer),hl \ save the updated IP
  de incp \ de=pfa
  e c ld
  d b ld \ bc=pfa
do_colon.end: \ XXX TMP for debugging
  jpnext

\ ----------------------------------------------
  _colon_header noname_,":NONAME",immediate

  \ [Code from the Afera library.]

  \ XXX TODO move to the disk? problem: do_colon

  question_exec_ __
  smudge_ __  \ deactivate the effect of the next `smudge` in `;`
  here_ __ \ cfa
  store_csp_ __
  lit_ __ do_colon __ comma_ __ \ create the code field
  right_bracket_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header semicolon_,";",immediate

  question_csp_ __
  compile_ __ semicolon_s_ __
  smudge_ __
  left_bracket_ __
  semicolon_s_ __

\ ----------------------------------------------
  _header noop_,"NOOP"

\ doc{
\
\ noop  ( -- )
\
\ }doc

  next __ \ code field

\ ----------------------------------------------
  _colon_header constant_,"CONSTANT"

  create_ __ comma_ __
  paren_semicolon_code_ __
$ constant do_constant
  de incp    \ de=pfa
  ex de,hl  \ hl=pfa
  jp fetch.hl

\ ----------------------------------------------
  _colon_header variable_,"VARIABLE"

\ doc{
\
\ variable ( "name" -- )  \ ANS Forth
\
\ Parse _name_.  Create a definition for _name_ with the
\ execution semantics defined below. Reserve one cell of data
\ space.
\
\    _name_ is referred to as a variable.
\
\          name Execution: ( -- a )
\
\    _a_ is the address of the reserved cell. A program is
\    responsible for initializing the contents of the reserved
\    cell.
\
\ }doc

  create_ __ cell_ __ allot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header user_,"USER"

  \ XXX TODO -- Use only one byte for storage,
  \ but defining `cconstant` only for this does not seem a good idea.

  constant_ __
  paren_semicolon_code_ __
$ constant do_user
\  _z80_border_wait 5 \ XXX INFORMER
  de incp      \ de=pfa
  ex de,hl
  ld e,(hl)
  ld d,0x00   \ de = index of the user variable
  ld hl,(user_variables_pointer)
  add hl,de   \ hl= address of the user variable
\  _z80_border_wait 6 \ XXX INFORMER
  jp push_hl

\ ----------------------------------------------
  _constant_header msg_scr_,"MSG-SCR"

\ doc{
\
\ msg-scr  ( -- n )
\
\ Constant: Screen where the error messages start.
\
\ }doc

\ Idea taken from lina ciforth.

  0x0004 __

\ ----------------------------------------------
  _constant_header zero_,"0"

  0x0000 __

\ ----------------------------------------------
  _constant_header one_,"1"

  0x0001 __

\ ----------------------------------------------
  _constant_header two_,"2"

  0x0002 __

\ ----------------------------------------------
  _code_header false_,"FALSE"

\ doc{
\
\ false  ( -- f )
\
\ }doc

  ld hl,false
  jp push_hl

\ ----------------------------------------------
  _code_header true_,"TRUE"

\ doc{
\
\ true  ( -- t )
\
\ }doc

  ld hl,true_flag
  jp push_hl

\ ----------------------------------------------
  _constant_header b_l_,"BL"

\ doc{
\
\ bl  ( -- n )
\
\ }doc

  space_char __

\ ----------------------------------------------
  _constant_header c_slash_l_,"C/L"

  characters_per_line __

\ ----------------------------------------------
  _constant_header l_slash_scr_,"L/SCR"

  lines_per_screen __

\ ----------------------------------------------
  _constant_header disk_buffer_,"DISK-BUFFER"

  disk_buffer __

\ ----------------------------------------------
  _constant_header b_slash_buf_,"B/BUF"

  data_bytes_per_buffer __

\ ----------------------------------------------
  _constant_header b_slash_scr_,"B/SCR"

  blocks_per_screen __

\ ----------------------------------------------
  _constant_header scr_slash_disk_,"SCR/DISK"

  screens_per_disk __

\ ----------------------------------------------
  _constant_header hash_vocs_,"#VOCS"

  max_search_order __

\ ----------------------------------------------
  _colon_header plus_origin_,"+ORIGIN"

\ doc{
\
\ +origin  ( n -- a )  \ fig-Forth
\
\ Leave the memory address relative by _n_ to the origin
\ parameter area.  _n_ is the minimum address unit, either byte
\ or word.  This definition is used to access or modify the
\ boot-up parameters at the origin area.
\
\ }doc

  lit_ __ origin __ plus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _user_variable_header sp0_,"SP0"

  0x00 _

\ ----------------------------------------------
  _user_variable_header rp0_,"RP0"

  0x02 _

\ ----------------------------------------------
  _user_variable_header width_,"WIDTH"

  \ XXX TODO normal variable

  0x06 _

\ ----------------------------------------------
  _user_variable_header warning_,"WARNING"

  0x08 _

\ ----------------------------------------------
  _user_variable_header dp_,"DP"

  \ XXX TODO why this is a user variable?
  \ XXX TODO normal variable

  0x0C _

\ ----------------------------------------------
  _variable_header voc_link_,"VOC-LINK"

  assembler_vocabulary_link __ \ link to the latest vocabulary defined

\ ----------------------------------------------
  _user_variable_header blk_,"BLK"

  \ XXX TODO normal variable

  0x10 _

\ ----------------------------------------------
  _user_variable_header to_in_,">IN"

  \ XXX TODO normal variable

  0x12 _

\ ----------------------------------------------
  _user_variable_header out_,"OUT"

  \ XXX TODO In Forth 83 it's a user variable too, but it's called `#out`.
  \ XXX TODO rename to `#emitted`
  0x14 _

\ ----------------------------------------------
  _user_variable_header scr_,'SCR' \ XXX OLD -- used by `list`

  0x16 _

\ ----------------------------------------------
  _user_variable_header context_,"CONTEXT"

  0x2A _

\ ----------------------------------------------
  _user_variable_header current_,"CURRENT"

  0x1C _

\ ----------------------------------------------
  _user_variable_header state_,"STATE"

  0x1E _

\ ----------------------------------------------
  _user_variable_header base_,"BASE"

  0x20 _

\ ----------------------------------------------
  _user_variable_header dpl_,"DPL"

  0x22 _

\ ----------------------------------------------
  _user_variable_header fld_,"FLD"

  0x24 _

\ ----------------------------------------------
  _user_variable_header csp_,"CSP"

  0x26 _

\ ----------------------------------------------
  _user_variable_header r_hash_,"R#"

  \ XXX OLD
  0x28 _

\ ----------------------------------------------
  _user_variable_header hld_,"HLD"

  0x1A _

\ ----------------------------------------------
  _constant_header tib_,"TIB"

\ doc{
\
\ tib  ( -- ca )  \ ANS-Forth
\
\ Address of the terminal input buffer.
\
\ }doc

  terminal_input_buffer __

\ ----------------------------------------------
  _variable_header number_tib_,"#TIB"

  bytes_per_terminal_input_buffer __

\ ----------------------------------------------
  _colon_header recurse_,"RECURSE",immediate

\ doc{
\
\ recurse  ( -- )  \ ANS Forth
\
\ }doc

  latest_ __ nfa_to_cfa_ __ compile_comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header one_plus_,"1+"

  hl pop
  hl incp
  jp push_hl

\ ----------------------------------------------
  _code_header two_plus_,"2+"

  hl pop
  hl incp
  hl incp
  jp push_hl

\ ----------------------------------------------
  _code_alias_header cell_minus_,"CELL-",,two_minus_

\ ----------------------------------------------
  _code_alias_header cell_plus_,"CELL+",,two_plus_

\ ----------------------------------------------
  _code_header one_minus_,"1-"

  hl pop
  hl decp
  jp push_hl

\ ----------------------------------------------
  _code_header two_minus_,"2-"

  hl pop
  hl decp
  hl decp
  jp push_hl

\ ----------------------------------------------
  _code_header two_star_,"2*"

\ doc{
\
\ 2*  ( x1 -- x2 )  \ ANS Forth
\
\ _x2_ is the result of shifting _x1_ one bit toward the
\ most-significant bit, filling the vacated least-significant
\ bit with zero.
\
\ This is the same as `1 lshift`.
\
\ }doc

  \ [Code from DZX-Forth. Documentation partly based on lina
  \ ciforth.]

  hl pop
  add hl,hl
  jp push_hl

\ ----------------------------------------------
  _code_alias_header cells_,"CELLS",,two_star_

\ ----------------------------------------------
  _constant_header cell_,"CELL"

  0x0002 __

\ ----------------------------------------------
  _code_header two_slash_,"2/"

\ doc{
\
\ 2/  ( x1 -- x2 )  \ ANS Forth
\
\ _x2_ is the result of shifting _x1_ one bit toward the
\ least-significant bit, leaving the most-significant bit
\ unchanged.
\
\ This is the same as `s>d 2 fm/mod swap drop`. It is not the same
\ as `2 /`, nor is it the same as `1 rshift`.
\
\ }doc

  \ [Code from Spectrum Forth-83. Documentation partly based on lina
  \ ciforth.]

  hl pop
  sra h
  rr l
  jp push_hl

\ ----------------------------------------------
  _colon_header here_,"HERE"

  dp_ __ fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header allot_,"ALLOT"

  dp_ __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header s_comma_,"s,"

\ doc{
\ s,  ( ca len -- )
\ }doc

  dup_ __ c_comma_ __ tuck_ __ here_ __ swap_ __ cmove_ __ allot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header comma_,","

  here_ __ store_ __ two_ __ allot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header compile_comma_,"COMPILE,"

\ doc{
\
\ compile,  ( cfa -- )
\
\ }doc

  question_comp_ __ comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header c_comma_,"C,"

  here_ __ c_store_ __ one_ __ allot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header minus_,"-"

  de pop
  hl pop
  a and
  sbc hl,de
  jp push_hl

\ ----------------------------------------------
  _code_header not_equals_,"<>"

  de pop
  hl pop
  compare_de_hl_unsigned call
false_if_z: \ XXX entry not used yet
  jp z,false_pfa
  jp true_pfa

\ ----------------------------------------------
  _code_header equals_,"="

  de pop
  hl pop
  compare_de_hl_unsigned call
true_if_z: \ XXX entry not used yet
  jp z,true_pfa
  jp false_pfa

\ ----------------------------------------------
  _code_header less_than_,"<"

  de pop
  hl pop
$ constant is_de_less_than_hl
  compare_de_hl_signed call
  size_optimization? [if]
    jp true_if_cy
  [else]
    jp c,true_pfa
    jp false_pfa
  [then]

\ ----------------------------------------------
  _code_header u_greater_than_,"U>"

  hl pop
$ constant u_greater_than.hl
  de pop
  jp u_less_than.de_hl

\ ----------------------------------------------
  _code_header u_less_than_,"U<"

  de pop
  hl pop
$ constant u_less_than.de_hl
  compare_de_hl_unsigned call
size_optimization? [if]
    jp true_if_cy
[else]
    jp c,true_pfa
    jp false_pfa
[endif]

\ ----------------------------------------------
  _code_header greater_than_,">"

  hl pop
  de pop
  jp is_de_less_than_hl

\ ----------------------------------------------
  _code_header rot_,"ROT"

  de pop
  hl pop
  ex (sp),hl
  jp push_hlde

\ ----------------------------------------------
  _colon_header space_,"SPACE"

  b_l_ __ emit_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_dup_,"?DUP"

  dup_ __
  zero_branch_,question_dup.end __
  dup_ __
$ constant question_dup.end
  semicolon_s_ __

\ ----------------------------------------------
  _code_alias_header lfa_to_nfa_,"LFA>NFA",,two_plus_

\ ----------------------------------------------
  _colon_header trail_,"TRAIL"

\ doc{
\
\ trail ( -- nfa )
\
\ Leave the name field address of the topmost word in the
\ `context` vocabulary.
\
\ }doc

  context_ __ fetch_ __ fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header latest_,"LATEST"

\ doc{
\
\ latest ( -- nfa )  \ ANS Forth
\
\ Leave the name field address of the topmost word in the
\ `current` vocabulary.
\
\ }doc

  current_ __ fetch_ __ fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header pfa_to_lfa_,"PFA>LFA"

  pfa_to_cfa_ __ cfa_to_nfa_ __

\ ----------------------------------------------
  _code_alias_header pfa_to_cfa_,"PFA>CFA",,two_minus_

\ ----------------------------------------------
  _code_alias_header cfa_to_pfa_,"CFA>PFA",,two_plus_

\ ----------------------------------------------
  _colon_header pfa_to_nfa_,"PFA>NFA"

  pfa_to_cfa_ __ cfa_to_nfa_ __
  semicolon_s_ __

\ ----------------------------------------------

\ doc{
\
\ cfa>nfa  ( cfa -- nfa )
\
\ Warning: No check is done where cfa belongs to a definition
\ created with `:noname`.
\
\ }doc

\ XXX FIXME -- make it return 0 if cfa has no name associated

  _code_header cfa_to_nfa_,"CFA>NFA"

  ld e,names_bank
  bank.e call \ page the memory bank
  de pop \ cfa
  bc push \ save Forth IP
  ld b,0
  ld hl, names_bank_address-4

$ constant cfa_to_nfa.begin_0
  \ hl = address of the cfa pointer
  hl incp
$ constant cfa_to_nfa.begin_1
  hl incp
  hl incp
  hl incp
  ld a,(hl) \ name field byte length
  and max_word_length_bit_mask \ name length
  a c ld \ name length
  c inc  \ plus the length byte
  add hl,bc \ point to the cfa pointer

  ld a,(hl) \ low byte of cfa
  cp e \ equal?
  jr nz,cfa_to_nfa.begin_0 \ not equal
  hl incp
  ld a,(hl) \ high byte of cfa
  cp d \ equal?
  jr nz,cfa_to_nfa.begin_1 \ not equal
  \ cfa found
  ld c,3
  add hl,bc \ nfa

  ld e,default_bank
  bank.e call \ page the default memory bank

  bc pop \ restore Forth IP
  jp push_hl

\ ----------------------------------------------

  _code_alias_header cfap_to_lfa_,"CFAP>LFA",,two_plus_

\ ----------------------------------------------

  _code_alias_header nfa_to_lfa_,"NFA>LFA",,two_minus_

\ ----------------------------------------------
  _colon_header c_store_bank_,"C!BANK"

\ doc{
\
\ c!bank  ( b a n -- )
\
\ Store _b_ into address _a_ of bank _n_.
\
\ }doc

  \ XXX 11 bytes
  bank_ __ c_store_ __
  _default_bank
  semicolon_s_ __

  \ XXX TODO
  \ de pop
  \ c_store_bank.e:
  \ bank.e call
  \ hl pop
  \ ld l,(hl)
  \ ld h,0
  \ ld e,default_bank
  \ bank.e call
  \ jp pushhl

\ ----------------------------------------------
  _colon_header store_bank_,"!BANK"

\ doc{
\
\ !bank  ( x a n -- )
\
\ Store _x_ into address _a_ of bank _n_.
\
\ }doc

  \ XXX 11 bytes
  bank_ __ store_ __
  _default_bank
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header c_fetch_bank_,"C@BANK"

\ doc{
\
\ c@bank  ( a n -- b )
\
\ Fetch the 8-bit content of address _a_ of the bank _n_.
\
\ }doc

  \ XXX 11 bytes
  bank_ __ c_fetch_ __
  _default_bank
  semicolon_s_ __

  \ XXX 15 bytes
  \ de pop
  \ c_fetch_bank.e:
  \ bank.e call
  \ hl pop
  \ ld l,(hl)
  \ ld h,0
  \ ld e,default_bank
  \ bank.e call
  \ jp pushhl

\ ----------------------------------------------
  _colon_header fetch_bank_,"@BANK"

\ doc{
\
\ @bank  ( a n -- x )
\
\ Fetch the 16-bit content of address _a_ of bank _n_.
\
\ }doc

  \ XXX 11 bytes
  bank_ __ fetch_ __
  _default_bank
  semicolon_s_ __

  \ XXX 17 bytes
  \ de pop
  \ fetch_bank.e
  \ bank.e call
  \ hl pop
  \ ld a,(hl)
  \ hl incp
  \ ld h,(hl)
  \ a l ld
  \ ld e,default_bank
  \ bank.e call
  \ jp pushhl

\ ----------------------------------------------
  _colon_header c_fetch_n_,"C@N"

\ doc{
\
\ c@n  ( a -- x )
\
\ Fetch from the _a_ address of the names bank.
\
\ }doc

  \ XXX 7 bytes:
  names_bank _literal
  c_fetch_bank_ __
  semicolon_s_ __

  \ XXX 5 bytes
  \ ld e,names_bank
  \ jp c_fetch_bank.e

\ ----------------------------------------------
  _colon_header fetch_n_,"@N"

\ doc{
\
\ @n  ( a -- x )
\
\ Fetch from the _a_ address of the names bank.
\
\ }doc

  \ XXX 7 bytes:
  names_bank _literal
  fetch_bank_ __
  semicolon_s_ __

  \ XXX 5 bytes
  \ ld e,names_bank
  \ jp fetch_bank.e

\ ----------------------------------------------
  _colon_header c_store_n_,"C!N"

\ doc{
\
\ c!n  ( c a -- )
\
\ Store _c_ into the _a_ address of the names bank.
\
\ }doc

  \ XXX 7 bytes:
  names_bank _literal
  c_store_bank_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header store_n_,"!N"

\ doc{
\
\ !n  ( x a -- )
\
\ Store _x_ into the _a_ address of the names bank.
\
\ }doc

  \ XXX 7 bytes:
  names_bank _literal
  store_bank_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header nfa_to_cfa_,"NFA>CFA"

  4 _literal
  minus_ __ fetch_n_ __
  semicolon_s_ __

\ ----------------------------------------------
\ doc{
\
\ nfa>string  ( nfa -- ca len )
\
\ }doc

  _colon_header nfa_to_string_,"NFA>STRING"

  _names_bank
  count_ __
  max_word_length_bit_mask _literal
  and_ __
  save_string_ __
  _default_bank
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header store_csp_,"!CSP"

  sp_fetch_ __ csp_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_error_,"?ERROR"

  swap_ __
  zero_branch_,question_error.no_error __
  error_ __
  semicolon_s_ __

$ constant question_error.no_error
  drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header comp_question_,"COMP?"

  state_ __ fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_comp_,"?COMP"

  comp_question_ __ zero_equals_ __
  _question_error error.compilation_only
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_exec_,"?EXEC"

  comp_question_ __
  _question_error error.execution_only
  semicolon_s_ __

\ ----------------------------------------------
1 [if]
  \ XXX TODO -- remove when the security is removed
  _colon_header question_pairs_,"?PAIRS"

  not_equals_ __
  _question_error error.conditionals_not_paired
  semicolon_s_ __
[then]

\ ----------------------------------------------
  _colon_header question_csp_,"?CSP"

  sp_fetch_ __ csp_ __ fetch_ __ not_equals_ __
  _question_error error.definition_not_finished
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_loading_,"?LOADING"

  blk_ __ fetch_ __ zero_equals_ __
  _question_error error.loading_only
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header compile_,"COMPILE"

  question_comp_ __
  from_r_,dup_,two_plus_,to_r_,fetch_,compile_comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header postpone_,"POSTPONE",immediate

\ doc{
\
\ postpone ( "name" -- )  \ ANS Forth, C I
\
\ Skip leading space delimiters. Parse name delimited by a
\ space. Find name. Append the compilation semantics of _name_ to
\ the current definition.
\
\ }doc

  defined_ __ \ ( ca 0 | cfa 1 | cfa -1 )
  dup_,question_defined_ __ \ error if not found
  zero_less_than_ __ \ non-immediate word?
  zero_branch_,postpone.end __
  \ Non-immediate word.
  compile_,compile_ __ \ compile `compile`
$ constant postpone.end
  compile_comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header left_bracket_,"[",immediate

  state_ __ off_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header right_bracket_,"]"

  state_ __ on_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header smudge_,"SMUDGE"


\ doc{
\
\ smudge  ( -- )
\
\ Toggle the "smudge bit" in a definitions' name field. This
\ prevents an uncompleted definition from being found during
\ dictionary searches, until compiling is completed without
\ error.
\
\ }doc

  latest_ __
  smudge_bit_mask _literal
  \ XXX TODO factor `toggle-names'?
  _names_bank
  toggle_ __
  _default_bank
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header hex_,"HEX"

  0x10 _literal
  base_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header decimal_,"DECIMAL"

  0x0A _literal
  base_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_semicolon_code_,"(;CODE)"

\ doc{
\
\ (;code)  ( -- )
\
\ The run-time procedure compiled by `;code`. Rewrite the code
\ field of the most recently defined word to point to the
\ following machine code sequence.
\
\ }doc

\ : (;code)       --
\   r>        \ Pop the address of the next instruction off the return stack,
\             \ which is the starting address of the run-time code routine.
\   latest    \ Get the name field address of the word under construction.
\   nfa>cfa ! \ Find its code field address and store in it the address of
\             \ the code routine to be executed at run-time.
\   ;

  from_r_ __ latest_ __ nfa_to_cfa_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header semicolon_code_,";CODE",immediate

\ XXX TODO -- documentation
\ doc{
\
\ ;code  ( -- )
\
\ Stop compilation and terminate a new defining word by
\ compiling the run-time routine `(;code)`.  Assemble the
\ assembly mnemonics following.
\
\ }doc

  question_csp_ __
  compile_ __ paren_semicolon_code_ __
  asm_ __
  left_bracket_ __ smudge_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header does_,"DOES>",immediate

  compile_ __ paren_semicolon_code_ __
  0xCD _literal \ Z80 opcode for "call"
  c_comma_ __ \ compile it
  lit_ __ do_does __ comma_ __ \ compile the routine address
  semicolon_s_ __

$ constant do_does
  \ Save the IP in the return stack.
  ld hl,(return_stack_pointer)
  hl decp
  ld (hl),b
  hl decp
  ld (hl),c
  ld (return_stack_pointer),hl
  \ Pop the address of the run-time routine
  \ (put there bye `call do_does`) in IP.
  bc pop \ new Forth IP
  \ Push the pfa.
  de incp  \ de=pfa
  de push
  \ Execute the run-time routine.
  jpnext

\ ----------------------------------------------
  _code_header count_,"COUNT"

  \ Code from DZX-Forth.

  de pop
  ld a,(de)
  de incp
  de push
  jp push_a

\ ----------------------------------------------
  _colon_header bounds_,"BOUNDS"

  over_ __ plus_ __ swap_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header type_,"TYPE"

  \ XXX TODO Rewrite in Z80, after the ROM routine.

  question_dup_ __
  zero_branch_,type.empty_string __

  bounds_ __
  paren_do_ __
$ constant type.do
  i_ __ c_fetch_ __ emit_ __
  paren_loop_,type.do __ \ loop
  semicolon_s_ __

$ constant type.empty_string
  drop_ __
$ constant type.end
  semicolon_s_ __

\ ----------------------------------------------

  _code_header minus_trailing_,"-TRAILING"

  de pop
  hl pop
  hl push
  add hl,de
  ex de,hl
  \ de = address after the string
  \ hl = length of the string
$ constant minus_trailing.begin
  l a ld
  h or \ exhausted?
  jp z,push_hl
  de decp \ next char
  ld a,(de)
  cp ' ' \ space?
  jp nz,push_hl
  hl decp \ new length
  jp minus_trailing.begin \ repeat

\ ----------------------------------------------
  \ _colon_header paren_dot_quote_,"(.\")" \ XXX FIXME as error
  _colon_header paren_dot_quote_,"(.\x22)"

  r_fetch_ __ count_ __ \ ( ca len )
  dup_ __ one_plus_ __ from_r_ __ plus_ __ to_r_ w __  \ skip the string after return
  type_ __
  semicolon_s_ __

\ ----------------------------------------------
  \ _colon_header dot_quote_,".\"",immediate \ XXX FIXME as error
  _colon_header dot_quote_,".\x22",immediate

  '"' _literal
  parse_ __ \ ( ca len )
  comp_question_ __
  zero_branch_ __ dot_quote.interpreting __ 
  \ Compiling.
  compile_ __ paren_dot_quote_ __ s_comma_ __
  exit_ __
$ constant dot_quote.interpreting
  type_ __
  semicolon_s_ __

\ ----------------------------------------------
  _variable_header span_,"SPAN"

\ doc{
\
\ span  ( -- a )  \ Forth-83
\
\ The address of a variable containing the count of characters
\ actually received and stored by the last execution of
\ `expect`.
\
\ }doc

  0 __

\ ----------------------------------------------
  _colon_header accept_,"ACCEPT"

\ doc{
\
\ accept  ( ca1 len1 -- len2 )  \ ANS Forth
\
\ }doc

  \ XXX TODO -- not finished

  \ span_ __ off_ __
  \ question_dup_ __
  \ zero_branch_ __ accept.end __

  \ swap_ __ \ ( len ca )
\ accept.begin: \ ( len ca )
  \ key_ __ dup_ __ \ ( len ca c c )

  \ c_lit_ __
  \ delete_char _
  \ equals_ __ \ delete key?
  \ zero_branch_ __ accept.maybe_carriage_return __
  \ \ Delete key ( len ca c )
  \ drop_ __
  \ dup_ __ i_ __ equals_ __ \ cursor at the start position?
  \ dup_ __ \ ( len ca f f )
  \ \ XXX TODO adapt this when true=-1
  \ from_r_ __ two_minus_ __ plus_ __ to_r_ __ \ update the index
  \ question_branch_ __ accept.loop __ \ nothing to delete
  \ \ b_l_ __ i_ __ c_store_ __ ; update the buffer ; XXX OLD
  \ c_lit_ __
  \ backspace_char _
  \ branch_ __ accept.emit __

\ accept.maybe_carriage_return: \ ( len ca c )
  \ dup_ __
  \ c_lit_ __
  \ carriage_return_char _
  \ equals_ __ \ carriage return?
  \ zero_branch_ __ accept.ordinary_key __
  \ \ Carriage return ( len ca c )
  \ exhaust_ __
  \ drop_ __ b_l_ __
  \ branch_ __ accept.emit __

\ accept.ordinary_key: \ ( len ca c )
  \ dup_ __
\ accept.store: \ ( len ca c c | len ca c 0 )
  \ i_ __ c_store_ __
\ accept.emit: \ ( len ca c )
  \ emit_ __

  \ \ ( len ca )
  \ i_ __ over_ __ minus_ __ span_ __ store_ __ \ update `span`
  \ paren_loop_ __ accept.do __
\ accept.end:
  \ drop_ __
  \ span_ __ fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header expect_,"EXPECT"

\ doc{
\
\ expect  ( ca len -- )  \ Forth-83
\
\ Transfer characters from the terminal to address _ca_, until a
\ "return" or _len_ characters have been received.
\
\ The transfer begins at addr proceeding towards higher
\ addresses one byte per character until either a "return" is
\ received or until _len_ characters have been transferred.

\ No more than _len_ characters will be stored.  The "return" is
\ not stored into memory.  No characters are received or
\ transferred if _len_ is zero.  All characters actually
\ received and stored into memory will be displayed, with the
\ "return" displaying as a space.

\ \ }doc

  span_ __ off_ __
  question_dup_ __
  zero_branch_ __ expect.end __

  bounds_ __ tuck_ __ \ ( ca ca+len ca )
  paren_do_ __
expect.do: \ ( ca )
  xkey_ __ dup_ __ \ ( ca c c )

  delete_char _literal
  equals_ __ \ delete key?
  zero_branch_ __ expect.maybe_carriage_return __
  \ Delete key ( ca c )
  drop_ __
  dup_ __ i_ __ equals_ __ \ cursor at the start position?
true_flag -1 = [if]
  \ XXX TODO simplify
  abs_ __
[then]
  dup_ __ \ ( ca f f )
  from_r_ __ two_minus_ __ plus_ __ to_r_ __ \ update the index
  question_branch_ __ expect.loop __ \ nothing to delete
  \ b_l_ __ i_ __ c_store_ __ \ update the buffer ; XXX OLD
  backspace_char _literal
  branch_ __ expect.emit __

expect.maybe_carriage_return: \ ( ca c )
  dup_ __
  carriage_return_char _literal
  equals_ __ \ carriage return?
  zero_branch_ __ expect.control_char __
  \ Carriage return ( ca c )
  exhaust_ __
  drop_ __ b_l_ __
  branch_ __ expect.emit __

expect.control_char: \ ( ca c )
  dup_ __ b_l_ __ less_than_ __ \ control char?
  zero_branch_ __ expect.ordinary_key __
  \ Control char ( ca c )
  drop_ __
  branch_ __ expect.do __

expect.ordinary_key: \ ( ca c )
  dup_ __
expect.store: \ ( ca c c | ca c 0 )
  i_ __ c_store_ __
expect.emit: \ ( ca c )
  emit_ __

expect.loop: \ ( ca )
  i_ __ over_ __ minus_ __ span_ __ store_ __ \ update `span`
  paren_loop_ __ expect.do __
$ constant expect.end
  drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header query_,"QUERY"

\ doc{
\
\ query  ( -- )  \ fig-Forth
\
\ XXX TODO description
\
\ Make the user input device the input source. Receive input
\ into the terminal input buffer, replacing any previous
\ contents. Make the result, whose address is returned by `tib`,
\ the input buffer. `>in` to zero.
\
\ Input 80 characters of text (or until a "return") from the
\ operators terminal. Text is positioned at the address
\ contained in TIB with IN set to zero.
\
\ Transfer characters from the terminal to address contained in
\ returned by `tib`, until a "return" or the count contained in
\ the `#tib` variable have been received. One or more nulls are
\ added at the end of the text.
\
\ }doc

  tib_ __ dup_ __
  number_tib_ __ fetch_ __
  two_dup_ __ blank_ __ \ clean the input buffer
  expect_ __
  span_ __ fetch_ __ plus_ __ stream_end_ __
  to_in_ __ off_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header x_,0,immediate

\ doc{

\ x  ( -- )

\ This is pseudonym for the "null" or dictionary entry for a
\ name of one character of ascii null. It is the execution
\ procedure to terminate interpretation of a line of text from
\ the terminal or within a disk buffer, as both buffers always
\ have a null word at the end.
\
\ In the fig-Forth model a null character is used to detect the
\ end of the buffers.  Therefore the scanning words must treat
\ the null character as a special unconditional delimiter, and
\ do other tricks in order to simulate the null character found
\ is a parsed null word.
\
\ In Solo Forth the scanning words does not treat the null char
\ apart; instead, an actual null word (a null character
\ surrounded by spaces) is put after the buffers (disk buffers
\ and `tib`). Therefore the null word is parsed normally as any
\ other word. No need to treat the null character as a special
\ delimiter. This new method is compatible with the original
\ fig-Forth parsing words.

\ }doc

  blk_ __ fetch_ __ \ input stream from disk?
  zero_branch_ __ x.exit __ \ if not, branch
  \ From disk.
  one_ __ blk_ __ plus_store_ __ \ next disk buffer
  to_in_ __ off_ __ \ clear `in`, preparing parsing of input text

  \ XXX WARNING -- The following check of the last block is
  \ specific for 2 blocks per screen; the generic slower check
  \ would be `blk @ b/scr 1- and`.

  blk_ __ fetch_ __ one_ __ and_ __ \ was it the last block of the screen?
  question_branch_ __ x.end __ \ if not, branch
  \ Last block of the screen.
  question_exec_ __ \ error if not executing
$ constant x.exit

  \ The top item on the return stack is thrown away.  The interpreter
  \ will not continue to execute the `?stack` instruction that follows
  \ `execute` in `interpret`, but will return to the next higher level
  \ of nesting and execute the next word after `interpret` in the Forth
  \ loop.  This is when the familiar "ok" message is displayed on the
  \ terminal, prompting the operator for the next line of commands.

  r_drop_ __
\  lit_ __ 7 __ border_ __ \ XXX INFORMER

$ constant x.end
  semicolon_s_ __

\ ----------------------------------------------
  _code_header fill_,"FILL"

  de pop \ e = char
$ constant fill.e
  c l ld
  b h ld \ the Forth IP
  bc pop \ count
  ex (sp),hl \ save the Forth IP
$ constant fill.do
  b a ld
  c or
  _jump_z fill.end
  ld (hl),e
  hl incp
  bc decp
  jp fill.do
$ constant fill.end
  bc pop \ restore the Forth IP
  jpnext

\ ----------------------------------------------
  _code_header erase_,"ERASE"

  ld e,0
  jp fill.e

\ ----------------------------------------------
  _code_header blank_,"BLANK"

  ld e,space_char
  jp fill.e

\ ----------------------------------------------
  _colon_header hold_,"HOLD"

  lit_ __ -1 __ hld_ __ plus_store_ __ \ decrement `hld`
  hld_ __ fetch_ __ c_store_ __ \ store character into `pad`
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header pad_,"PAD"

  here_ __
  0x44 _literal
  plus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header stream_,"STREAM"

\ doc{
\
\ stream  ( -- ca )
\
\ ca = current parsing position in the stream source
\
\ }doc

  blk_ __ fetch_ __ question_dup_ __ \ from disk?
  zero_branch_ __ stream.terminal __
  block_ __ \ from disk
  branch_ __ stream.end __
$ constant stream.terminal
  tib_ __ \ from terminal
$ constant stream.end
  to_in_ __ fetch_ __ plus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header parsed_,"PARSED"

\ doc{
\
\ parsed  ( len -- )
\
\ Add the given _len_ plus 1 to `>in`.
\
\ }word

  one_plus_ __ to_in_ __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header parse_,"PARSE"

\ doc{
\
\ parse  ( c "text<c>" -- ca len )  \ ANS Forth
\
\ Parse _text_ delimited by the delimiter char _c_.
\
\ ca = address of the parsed string, within the input buffer
\ len = length of the parsed string
\
\ If the parse area was empty, the resulting string has a zero length.
\
\ }doc

  stream_ __ swap_ __ scan_ __ dup_ __ parsed_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header parse_name_,"PARSE-NAME"

\ doc{
\
\ parse-name  ( "name"  -- ca len )
\
\ }word

  stream_ __ dup_ __ to_r_ __ \ ( ca1 )
  b_l_ __ skip_ __ \ ( ca2 )
  dup_ __ from_r_ __ minus_ __ to_in_ __ plus_store_ __
  \ XXX TODO factor of `parse`?:
  b_l_ __ scan_ __ \ ( ca len )
  dup_ __ parsed_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header word_,"WORD"

\ doc{
\
\ word  ( c "<c...>text<c>" -- ca )  \ ANS Forth
\
\ c = delimiter char
\
\ Skip leading _c_ delimiters from the input stream.  Parse the
\ next text characters from the input stream, until a delimiter
\ _c_ is found, storing the packed character string beginning at
\ _ca_, as a counted string (the character count in the first
\ byte), and with one blank at the end.
\
\ }doc

  stream_ __ \ ( c a1 )
  dup_ __ to_r_ __
  over_ __ skip_ __ \ ( c a2 )
0 [if]
  hex_ __
  _echo 'In word after skip:' \ XXX INFORMER
  cr_ __ dot_s_ __ cr_ __ \ XXX INFORMER
  key_ __ drop_ __ \ XXX INFORMER
[then]
  dup_ __ from_r_ __ minus_ __ to_in_ __ plus_store_ __
  \ XXX TODO factor of `parse`:
  swap_ __ scan_ __ \ ( a2 len )
0 [if]
  hex_ __
  _echo 'In word after scan:' \ XXX INFORMER
  cr_ __ two_dup_ __ type_ __ \ XXX INFORMER
  cr_ __ dot_s_ __ cr_ __ \ XXX INFORMER
  key_ __ drop_ __ \ XXX INFORMER
[then]
  dup_ __ one_plus_ __ to_in_ __ plus_store_ __

  here_ __
  max_word_length+2 _literal
  blank_ __

  dup_ __ here_ __ c_store_ __ \ count byte
  here_ __ one_plus_ __ \ destination
  swap_ __ \ count
  cmove_ __ \ move the word
  here_ __

0 [if] \ XXX OLD
  ;space_ __ \ XXX INFORMER
  ;depth_ __ dot_ __ \ XXX INFORMER
  ;_echo 'blk ' \ XXX INFORMER
  ;blk_ __ fetch_ __ dot_ __ \ XXX INFORMER
  ;_echo 'in ' \ XXX INFORMER
  ;in_ __ fetch_ __ dot_ __ \ XXX INFORMER
  \ here_ __ count_ __ one_ __ ink_ __ type_ __ zero_ __ ink_ __ space_ __ ; XXX INFORMER
  ;key_ __ drop_ __ \ XXX INFORMER
[then]

  semicolon_s_ __
\ ----------------------------------------------
  _colon_header paren_number_,"(NUMBER)"

\ doc{
\
\ (number)  ( d1 ca1 -- d2 ca2 )  \ fig-Forth
\
\ Convert the ASCII text beginning at _ca1+l_ with regard to
\ `base`. The new constant is accumulated into double number _d1_,
\ being left as _d2_.  _ca2_ is the address of the first
\ unconvertable digit. Used by `number`.
\
\ }doc

paren_number.begin: \ begin
  one_plus_ __ \ address of the next digit
  dup_ __ to_r_ __ \ save the address
  c_fetch_ __ \ get the digit
  \ dot_s_ __ key_ __ drop_ __ \ XXX INFORMER
  \ dup_ __ dup_ __ cr_ __ dot_ __ emit_ __ \ XXX INFORMER
  base_ __ fetch_ __ digit_ __ \ convert the digit
  zero_branch_ __ paren_number.end __ \ while
  swap_ __ \ get the high order part of d1 to the top.
  base_ __ fetch_ __ u_m_star_ __ \ multiply by base constant
  drop_ __ \ drop the high order part of the product
  rot_ __ \ move the low order part of d1 to top of stack
  base_ __ fetch_ __ u_m_star_ __ \ multiply by base constant
  d_plus_ __ \ accumulate result into d1
  dpl_ __ fetch_ __ one_plus_ __ \ is DPL other than -1?
  zero_branch_ __ paren_number.decimal_point_done __
  \ DPL is not -1, a decimal point was encountered
  one_ __ dpl_ __ plus_store_ __ \ increment DPL, one more digit to right of decimal point
$ constant paren_number.decimal_point_done
  from_r_ __ \ pop addr1+1 back to convert the next digit
  branch_ __ paren_number.begin __ \ repeat
$ constant paren_number.end
  from_r_ __ \ address of the first non-convertable digit, a2.
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header number_,"NUMBER"

\ doc{
\
\ number  ( ca  -- d )  \ fig-Forth
\
\ Convert a counted character string left at _ca_, to a signed
\ .double number, using the current numeric base. If a decimal
\ point is encountered in the text, its position will be given
\ in `dpl`, but no other effect occurs. If numeric conversion is
\ not possible, an error message will be given.
\
\ }doc

  zero_ __ zero_ __ rot_ __ \ two zeros, initial constant of the double number
  dup_ __ one_plus_ __ c_fetch_ __ \ get the first digit
  '-' _literal
  equals_ __ \ is it a minus sign?
  dup_ __ to_r_ __ \ save the flag
true_flag -1 = [if]
  abs_ __
[then]
  plus_ __
  \ If the first digit is "-", the flag is 1,
  \ and addr+1 points to the second digit.
  \ If the first digit is not "-", the flag is
  \ 0.  addr+0 remains the same, pointing to
  \ the first digit.
  dw lit_,-1 \ initial constant of `dpl`
$ constant number.begin
  dpl_ __ store_ __
  paren_number_ __ \ convert one digit after another until an invalid char occurs
  dup_ __ c_fetch_ __ \ get the invalid digit
  \ dw dot_s_,key_,drop_; XXX INFORMER
  \ dw cr_,dup_,emit_; XXX INFORMER
  b_l_ __ equals_ __ \ is it a blank?
  question_branch_ __ number.a_blank __
  \ The invalid digit is not a blank.
  dup_ __ c_fetch_ __ \ get the invalid digit again
  '.' _literal
  not_equals_ __ \ not a decimal point?
  _question_error error.not_understood \ error if not
  \ Decimal point found, set `dpl` to zero next time.
  zero_ __
  branch_ __ number.begin __ \ repeat
$ constant number.a_blank
  drop_ __ \ discard address
  from_r_ __ \ pop the flag of "-" sign back
  zero_branch_ __ number.end __
  \ The first digit is a "-" sign.
  dnegate_ __
$ constant number.end
  semicolon_s_ __

\ ----------------------------------------------
  _code_header upper_,"UPPER"

\ doc{
\
\ upper  ( c -- c' )
\
\ }doc

  hl pop
  l a ld
  upper.a call
  a l ld
  jp push_hl

$ constant upper.a
  \ Convert the ASCII char in the 'a' register to uppercase.
  cp 'a'
  retc
  cp 'z'+1
  retnc
  xor 0x20 \ toggle bit 5
  ret

\ ----------------------------------------------
  _code_header uppers_,"UPPERS"

\ doc{
\
\ uppers  ( ca len -- )
\
\ }doc

  de pop
  hl pop
$ constant uppers.do
  d a ld
  e or
  jp z,next
  ld a,(hl)
  upper.a call
  ld (hl),a
  hl incp
  de decp
  jp uppers.do

\ ----------------------------------------------
  _colon_header defined_question_,"DEFINED?"

\ defined?  ( ca len -- wf )

  found_ __ nip_ __ zero_not_equals_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header undefined_question_,"UNDEFINED?"

\ doc{
\
\ undefined?  ( ca len -- wf )
\
\ }doc

  defined_question_ __ zero_equals_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header dollar_store_,"$!"

  \ [Code from DZX-Forth's `packed`.]

\ doc{
\
\ $!  ( ca1 len1 ca2 -- )
\
\ Store the string _ca1 len1_ as a counted string at _ca2_.  The
\ source and destination strings are permitted to overlap.
\
\ An ambiguous condition exists if _len1_ is greater than 255 or
\ the buffer at _ca2_ is less than _len1_+1 characters.
\
\ }doc

  \ XXX TODO rename? `s!`, `packed`, `pack`, `uncount`...

  exx
  de pop      \ de=ca2
  bc pop      \ c=len1
  hl pop      \ hl=ca1
  bc push     \ len1
  de push     \ ca2
  de incp
  move_block call
  hl pop      \ ca2
  de pop      \ e=len1
  ld (hl),e
  exx
  jpnext

\ ----------------------------------------------
  _colon_header find_,"FIND"

\ doc{
\
\ find  ( ca --- ca 0 | cfa 1 | cfa -1 )
\
\ Find the definition named in the counted string at _ca_. If
\ the definition is not found after searching all the
\ vocabularies in the search order, return _ca_ and zero.  If
\ the definition is found, return its _cfa_. If the definition
\ is immediate, also return one (1); otherwise also return
\ minus-one (-1).
\
\ }doc

  \ : find  ( ca --- ca 0 | cfa 1 | cfa -1 )
  \   #vocs 0 do
  \     context i cells + @  ?dup
  \     if  @ (find) ?dup if  unloop exit  then  then
  \   loop  false  ;

  hash_vocs_ __ zero_ __ paren_do_ __
$ constant find.do
  dw context_,i_,cells_,plus_,fetch_
  question_dup_ __ \ a vocabulary in the search order?
  zero_branch_ __ find.loop __ \ if not, next
  \ valid vocabulary in the search order
  fetch_ __ paren_find_ __ question_dup_ __ \ word found in the vocabulary?
  zero_branch_ __ find.loop __ \ if not, try the next vocabulary
  unloop_ __ exit_ __
$ constant find.loop
  paren_loop_ __ find.do __ false_ __
  semicolon_s_ __

\ ----------------------------------------------
  _variable_header find_dollar_,"FIND$"

  \ XXX TODO use an unused address above `pad` instead?
  \ XXX TODO rename to `word$`?
  \ XXX TODO use also in `word`?

  ds max_word_length+2

\ ----------------------------------------------
  _colon_header found_,"FOUND"

\ doc{
\
\ found  ( ca len --- ca 0 | cfa 1 | cfa -1 )
\
\ }doc

  \ XXX TODO factor

  find_dollar_ __
  max_word_length+2 _literal
  erase_ __ \ make sure there will be a null at the end
  find_dollar_ __ dollar_store_ __
  find_dollar_ __ count_ __ uppers_ __
  find_dollar_ __ find_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_abort_,"(ABORT)"

  abort_ __
  semicolon_s_ __

\ ----------------------------------------------
  _variable_header error_number_,"ERROR#"

  0 __

\ ----------------------------------------------
  _two_variable_header error_pos_,"ERROR-POS"

  0 __ 0 __

\ ----------------------------------------------
  _colon_header error_to_line_,"ERROR>LINE"

\ doc{
\
\ error>line  ( n1 -- n2 )
\
\ Convert an error number to its correspondent line offset. This
\ is used in order to skip the first line of screens and use
\ them as screen headers as usual.
\
\ }doc

  dup_ __ one_plus_ __ one_ __ paren_do_ __
$ constant error_to_number.do
  i_ __
  16 _literal
  mod_ __ zero_equals_ __
true_flag -1 = [if]
  abs_ __
[then]
  plus_ __
  paren_loop_ __ error_to_number.do __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header error_,"ERROR"

  dup_ __ error_number_ __ store_ __ \ save the error number
  warning_ __ fetch_ __ zero_less_than_ __ \ custom error routine?
  question_branch_ __ paren_abort_pfa __ \ if so, branch to it

$ constant error.message
  here_ __ count_ __ type_ __ \ last parsed word ; XXX TODO adapt to `parse-word`
  paren_dot_quote_ __
  2 _ '?' _ bl _
  message_ __
  sp0_ __ fetch_ __ sp_store_ __
  blk_ __ fetch_ __ question_dup_ __
  zero_branch_ __ error.end __
  to_in_ __ fetch_ __
  swap_ __
  error_pos_ __ two_store_ __
$ constant error.end
  quit_ __

\ ----------------------------------------------
  _colon_header id_dot_,"ID."

  nfa_to_string_ __ type_ __ space_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header header_,"HEADER"

\ header  ( "name" -- )

  \ XXX TODO -- make sure `current` is searched? else duplicated
  \ definitions would not be remarked.
  ;
  \ the only secure method is:
  ;
  \   get-order n>r also current @ context ! defined nr> set-order
  ;
  \ but that would make the compilation slower.
  \ i think it can be left to the programmer.

  defined_ __ \ ( ca 0 | cfa 1 | cfa -1 )
  abs_ __ star_ __ question_dup_ __ \ ( 0 | cfa cfa )
  zero_branch_ __ header.continue __
  \ The word is not unique.
  cfa_to_nfa_ __

  id_dot_ __
  _message error.not_unique
$ constant header.continue

    \ XXX TODO adapt to `parse-word`; now it works because
    \ `defined` still uses `word`, that leaves the string at
    \ `here`.

  here_ __ count_ __
  \ XXX TODO error if name is too long? (see lina)
  width_ __ fetch_ __ min_ __
  save_string_ __ tuck_ __ \ ( len ca len )
  _names_bank
  here_ __ comma_np_ __ \ store a pointer to the cfa
  latest_ __ comma_np_ __ \ link field
  \ Now `np` contains the address of the nfa.
  np_fetch_ __ dollar_store_ __ \ store the name
  np_fetch_ __ current_ __ fetch_ __ store_ __ \ update contents of `latest` in the current vocabulary
  smudge_ __ \ set the smudge bit and page the default bank
  one_plus_ __ np_ __ plus_store_ __ \ update the names pointer with the length+1
  here_ __ two_plus_ __ comma_ __ \ compile the pfa into code field
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header create_,"CREATE"

  header_ __ smudge_ __
  paren_semicolon_code_ __
$ constant do_create
  de incp  \ de=pfa
  de push
  jpnext

\ ----------------------------------------------
  _colon_header code_,"CODE"

  header_ __
  store_csp_ __
  also_ __ assembler_ __ asm_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header compare_,"COMPARE"

  \ ANS Forth
  \ Adapted from DZX-Forth

  \ XXX TODO do not use compare_strings_case_sensitive,
  \ because there will be no option to change it.
  \ Use shorter internal code instead.
  \ When case insensitive comparation is needed,
  \ `uppers` can be used.

  de pop      \ de = len2
  hl pop      \ hl = ca2
  ex (sp),hl  \ hl = len1 ; ( ca1 ca2 )
  d a ld
  cp h
  jr nz,compare.lengths
  e a ld
  cp l
$ constant compare.lengths
  \ cy = string2 is longer than string1?
  jr c,compare.ready
  ex de,hl
$ constant compare.ready
  \ de = length of the short string
  \ hl = length of the long string
  c l ld
  b h ld \ hl = Forth IP
  bc pop \ bc = ca2
  ex (sp),hl \ hl = ca1 ; save Forth IP
  af push \ save carry flag
compare.compare_strings: equ $+1 \ XXX not used
  compare_strings_case_sensitive call
  jr nz,compare.no_match

$ constant compare.match
  \ The smaller string matches.
  af pop \ restore flags
  jr compare.end

$ constant compare.no_match
  \ The smaller string does not match.
  bc pop \ useless carry flag

$ constant compare.end
  bc pop \ restore Forth IP
  ld hl,1
  jp c,push_hl
  hl decp \ 0
  jp z,push_hl \ string1 equals string2
  hl decp \ -1
  jp push_hl

$ constant compare_strings_case_sensitive
  \ Used by 'compare' and 'search'.
  \ Input:
  \   HL = a1
  \   BC = a2
  \   DE = len
  \ Output:
  \   Z = match?
  \ [Code from DZX-Forth.]
  e a ld
  d or
  retz
  ld a,(bc)
  cp (hl)
  retnz
  hl incp
  bc incp
  de decp
  jp compare_strings_case_sensitive

\ ----------------------------------------------
  _code_header search_,"SEARCH"

  \ search  ( ca1 len1 ca2 len2 -- ca3 len3 -1 | ca1 len1 0 )

  \ ANS Forth

  \ Adapted from DZX-Forth.
  ;
  \ XXX TODO do not use compare_strings_case_sensitive,
  \ because there will be no option to change it.
  \ Use shorter internal code instead.
  \ When case insensitive comparation is needed,
  \ `uppers` can be used.

  exx \ save Forth IP
  hl pop
  ld (search.string_2_len),hl
  l a ld
  h or \ len2 is zero?
  bc pop \ ca2
  hl pop \ len1
  ld (search.string_1_len),hl
  ex de,hl \ de = len1
  hl pop \ ca1
  ld (search.string_1_addr),hl
  jp z,search.match \ if len2 is zero, match
  hl decp
  de incp
$ constant search.1
  hl incp \ address of current char of string 1
  de decp \ remaining length of string 1
  e a ld
  d or \ end of string 1?
  jp z,search.no_match
\ XXX OLD -- already commented out in DX-Forth:
\ ld a,(bc)
\ cp  (hl)
\ jp nz,search.1
  de push
  bc push
  hl push
  ex de,hl
search.string_2_len equ $+1
  ld hl,0  \ length of the second string
  ex de,hl
  compare_strings_case_sensitive call
  hl pop
  bc pop
  de pop
  jp nz,search.1

$ constant search.match
  ld bc,true
$ constant search.end
  hl push
  de push
  bc push
  exx \ restore Forth IP
  jpnext

$ constant search.no_match
  ld bc,false
search.string_1_len equ $+1
  ld hl,0  \ length of the first string
  ex de,hl
search.string_1_addr equ $+1
  ld hl,0  \ address of the first string
  jp search.end

\ ----------------------------------------------
  _colon_header bracket_compile_,"[COMPILE]",immediate

  tick_ __ compile_comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header s_literal_,"SLITERAL",immediate

  \ : sliteral  ( ca len -- )  compile slit s,  \ immediate

  compile_ __ s_lit_ __ s_comma_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header c_literal_,"CLITERAL",immediate

\ doc{
\
\ cliteral  ( b -- )  \ I
\
\ If compiling, then compile the stack constant _b_ as a 8-bit literal.
\ `cliteral` does the same than `literal` but saves one byte of
\ dictionary space.
\
\ }doc

  \ XXX TODO -- `interpret` needs the old method of `literal`

  1 [if] \ XXX OLD
    comp_question_ __
    zero_branch_ __ c_literal.end __
    compile_ __ c_lit_ __ c_comma_ __
$ constant c_literal.end
  [else] \ XXX NEW
    question_comp_ __
    compile_ __ c_lit_ __ comma_ __
  [then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header literal_,"LITERAL",immediate

\ doc{
\
\ literal  ( n -- )  \ fig-Forth, I
\
\ If compiling, then compile the stack constant _n_ as a 16-bit literal.
\
\ }doc

  \ XXX TODO -- `interpret` needs the old method of `literal`

  1 [if] \ XXX OLD
    comp_question_ __
    zero_branch_ __ literal.end __
    compile_ __ lit_ __ comma_ __
$ constant literal.end
  [else] \ XXX NEW
    question_comp_ __
    compile_ __ lit_ __ comma_ __
  [then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header two_literal_,"2LITERAL",immediate

\ doc{
\
\ 2literal  ( d -- )  \ I
\
\ If compiling, then compile the stack constant _d_ as a 32-bit literal.
\
\ }doc

  \ XXX TODO -- `interpret` needs the old method of `literal`

  1 [if] \ XXX OLD
    comp_question_ __
    zero_branch_ __ two_literal.end __
    swap_ __ literal_ __ literal_ __
$ constant two_literal.end
  [else] \ XXX NEW
    \ XXX TODO -- dlit
    question_comp_ __
    swap_ __ literal_ __ literal_ __
  [then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header depth_,"DEPTH"

  dw sp_fetch_,sp0_,fetch_,minus_,lit_,-2,slash_
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_stack_,"?STACK"

\ doc{
\
\ ?stack  ( -- )  \ fig-Forth
\
\ Issue an error message if the stack is out of bounds.
\
\ }doc

  sp_fetch_ __
  sp0_ __ fetch_ __
  swap_ __ less_than_ __
  _question_error error.stack_empty
  sp_fetch_ __
  lit_ __ data_stack_limit __
  less_than_ __
  _question_error error.full_stack
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header interpret_,"INTERPRET"

\ doc{
\
\ interpret  ( -- )
\
\ The outer text interpreter which sequentially executes or
\ compiles text from the input stream (terminal or disk)
\ depending on `state`. if the word name cannot be found after a
\ search of the `context` search order it is converted to a
\ number according to the current `base`.  That also failing, an
\ error message echoing the name with a "?" will be given.
\
\ }doc

$ constant interpret.begin

  \ XXX TODO -- finish

  \ XXX TODO -- In order to change the behaviour of `literal`,
  \ `2literal` and `cliteral` (make them give an error in
  \ interpretation mode), `interpret` must be modified.


  question_stack_ __
  defined_ __ \ ( ca 0 | cfa 1 | cfa -1 )
  question_dup_ __ \ found?
  zero_branch_ __ interpret.word_not_found __

  \ Found ( cfa 1 | cfa -1 )
  \ Immediate word:     ( cfa  1 )
  \ Non-immediate word: ( cfa -1 )
  comp_question_ __
  \ ( cfa 1 state | cfa -1 state )
true_flag 1 = [if]
  \ Compiling an immediate word:     ( cfa  1 1 )
  \ Compiling a non-immediate word:  ( cfa -1 1 )
  \ Executing an immediate word:     ( cfa  1 0 )
  \ Executing a non-immediate word:  ( cfa -1 0 )
  negate_ __
[then]
  \ Compiling an immediate word:     ( cfa  1 -1 )
  \ Compiling a non-immediate word:  ( cfa -1 -1 )
  \ Executing an immediate word:     ( cfa  1  0 )
  \ Executing a non-immediate word:  ( cfa -1  0 )
  equals_ __ \ compiling a non-immediate word?
  zero_branch_ __ interpret.execute __

  \ Compiling a non-immediate word  ( cfa )
  compile_comma_ __
  branch_ __ interpret.begin __

$ constant interpret.execute
  \ Executing or immediate ( cfa )
  execute_ __
  branch_ __ interpret.begin __

$ constant interpret.word_not_found
  \ try to convert the text to a number
  \ ( ca )
  number_ __
  dpl_ __ fetch_ __ one_plus_ __ \ is there a decimal point?
  zero_branch_ __ interpret.16bit_number __
  \ decimal point detected, so it's a double, 32-bit, number
  two_literal_ __
  branch_ __ interpret.begin __

$ constant interpret.16bit_number
  \ dw lit_,1,border_,key_,drop_ \ XXX INFORMER
  \ no decimal point, so it's a 16-bit number
  drop_ __ \ discard high order part of the double number
  \ XXX TODO use `c_literal` for 8-bit values.
  literal_ __
  branch_ __ interpret.begin __

\ ----------------------------------------------
  _colon_header immediate_,"IMMEDIATE"

  latest_ __
  precedence_bit_mask _literal
  \ XXX TODO factor `toggle-names'?
  _names_bank
  toggle_ __
  _default_bank
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header vocabulary_,"VOCABULARY"

\ doc{
\
\ vocabulary  ( "name" -- )
\
\ Create a vocabulary with the parsed "name" as its name. The
\ run-time efect of `name` is to replace `context`, the top
\ vocabulary in the search order.
\
\ }doc

  create_ __
  zero_ __ comma_ __ \ space for the nfa of the latest word defined in the vocabulary
  here_ __ \ address of vocabulary link
  voc_link_ __ fetch_ __ comma_ __ \ compile the current content of `voc-link`
  voc_link_ __ store_ __ \ update `voc-link` with the link in this vocabulary

  paren_semicolon_code_ __

$ constant do_vocabulary
  do_does call

  \ The next words are to be executed when the vocabulary is invoked.
  \ _echo "Was here 1"
  \ dw lit_,3,border_,key_,drop_ \ XXX INFORMER
  context_ __
  \ _echo "Was here 2"
  \ dw lit_,4,border_,key_,drop_ \ XXX INFORMER
  store_ __
  \ _echo "Was here 3"
  \ dw lit_,5,border_,key_,drop_ \ XXX INFORMER
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header definitions_,"DEFINITIONS"

  context_ __ fetch_ __
  current_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_,"(",immediate

  ')' _literal
  \ XXX TODO use `skip`?
  parse_ __ two_drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header quit_,"QUIT"

  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  blk_ __ off_ __
  left_bracket_ __
$ constant quit.do
  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  rp0_ __ fetch_ __ rp_store_ __
  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  cr_ __
  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  query_ __
  \ XXX FIXME never reached before the crash
  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  interpret_ __
  comp_question_ __
  question_branch_ __ quit.do __
  paren_dot_quote_ __
  2 _ 'o' _ 'k' _
  branch_ __ quit.do __

show_version? [if]

\ ----------------------------------------------
  _constant_header version_release_,"VERSION-RELEASE"

  \ XXX TMP for debugging

  version_release_variable __

\ ----------------------------------------------
  _colon_header dot_version_,".VERSION"

  \ XXX TMP

  lit_ __ version_status_variable __ fetch_ __ emit_ __
  '-' _literal
  emit_ __
  lit_ __ version_branch_variable __ fetch_ __
  dw s_to_d_,less_hash_,hash_,hash_,hash_greater_,type_
  '-' _literal
  emit_ __
  lit_ __ version_release_variable __ two_fetch_ __
  \ XXX TODO use `du.` when available (it's in the library disk)
  less_hash_ __ hash_s_ __ hash_greater_ __ type_ __
  semicolon_s_ __

[then]

\ ----------------------------------------------
  _colon_header greeting_,"GREETING"

\ doc{
\
\ greeting  ( -- )
\
\ }doc

show_version? [if]
  \ XXX TODO
  paren_dot_quote_ __
\  _string "Solo Forth\r\x7F 2015 Marcos Cruz\r(programandala.net)\r"
  _string "Solo Forth\rVersion "
  \ XXX TMP show the version and the free dictionary memory:
  dot_version_ __ cr_ __
\  _string "\r\x7F 2015 Marcos Cruz\r(programandala.net)\r"
[else]
  \ XXX TODO adapt
  paren_dot_quote_ __
  db greeting.string_0_end-$-1
  db "Solo Forth",carriage_return_char
  db copyright_char," 2015 Marcos Cruz",carriage_return_char
  db "(programandala.net)",carriage_return_char
$ constant greeting.string_0_end
  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  \ XXX TMP show the free memory, during development only
  unused_ __ u_dot_ __
  paren_dot_quote_ __
  \ XXX TODO adapt
  db greeting.string_1_end-$-1
  db "bytes free"
$ constant greeting.string_1_end
[then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header abort_,"ABORT"


  sp0_ __ fetch_ __ sp_store_ __
$ constant boot
  noop_ __ \ patched by `turnkey` ; XXX OLD
  quit_ __

\ ----------------------------------------------
  _constant_header boot_,"BOOT"

  boot __

\ ----------------------------------------------
  _colon_header warm_,"WARM"

  \ sp0_ __ fetch_ __ sp_store_ __
  \ noop_ __ \ patched by `turnkey` ; XXX OLD
  \ quit_ __
  page_ __ abort_ __
  semicolon_s_ __

$ constant warm_start

  ld (system_stack_pointer),sp \ save the system stack pointer
\  XXX TODO this works too
\  ld hl,abort_
\  ld ix,next \ restore IX
\  jp next2

  common_start call
  warm_ __ \ XXX FIXME -- this works
\  abort_ __ \ XXX FIXME -- this crashes the system, why?

\ ----------------------------------------------
  _colon_header cold_,"COLD"

  \ Init the names pointer.
  lit_ __ names_pointer_init_value __ fetch_ __
  lit_ __ names_pointer __ store_ __

  \ Init the disk buffers.
  empty_buffers_ __

  \ Init the circular string buffer.
  empty_csb_ __

  \ Init the user variables.
  lit_ __ default_user_variables_start __ \ from
  lit_ __ user_variables_pointer __ fetch_ __ \ to
  default_user_variables_end-default_user_variables_start _literal \ length
  cmove_ __

  \ Restore the vocabularies to the default state.
  lit_ __ latest_nfa_in_root_voc.init_value __ fetch_ __
  lit_ __ root_pfa __ store_ __
  lit_ __ latest_nfa_in_forth_voc.init_value __ fetch_ __
  lit_ __ forth_pfa __ store_ __
  lit_ __ latest_nfa_in_assembler_voc.init_value __ fetch_ __
  lit_ __ assembler_pfa __ store_ __
  lit_ __ voc_link.init_value __ fetch_ __
  lit_ __ voc_link_pfa __ store_ __

  only_ __ forth_ __ definitions_ __  \ search order
  decimal_ __      \ base

  display_ __ colors0_ __ page_ __ greeting_ __

  abort_ __

$ constant cold_start
  ld (system_stack_pointer),sp \ save the system stack pointer
only_first_cold: \ XXX TMP -- temporary label
  move_name_fields_to_memory_bank call \ (only the first time)
latin1_charset_in_bank? [if]
  ld hl,charset_address-0x0100
  ld (sys_chars),hl
[then]
  common_start call
  cold_ __

$ constant common_start

  \ Common operations done by warm_start and cold_start.

  bc pop \ get the return address, that holds the cfa of `cold` or `warm`
  ld sp,(s0_init_value)
  a xor
  ld (iy+sys_df_sz_offset),a \ no lines at the bottom part of the screen
  ld ix,next \ restore IX
  jpnext \ jump to the cfa pointed by the BC register

\ ----------------------------------------------
  _code_header s_to_d_,"S>D"

\ doc{
\
\ s->d  ( n -- d )
\
\ Sign extend a single number _n_ to form a double number _d_.
\
\ }doc

\ dup 0<

  ld hl,0
  de pop
  d a ld
  a or
  jp p,push_hlde \ jump if positive
  hl decp
  jp push_hlde

\ ----------------------------------------------
  _colon_header plus_minus_,"+-"

\ doc{
\
\ +-  ( n1 n2 -- n3 )  \ fig-Forth
\
\ Apply the sign of n2 to n1, which is left as n3.
\
\ }doc

  zero_less_than_ __
  zero_branch_ __ plus_minus.end __
  negate_ __
$ constant plus_minus.end
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header d_plus_minus_,"D+-"

\ doc{
\
\ d+-  ( d1 n -- d2 )  \ fig-Forth
\
\ Apply the sign of _n_ to the double number _d1_, leaving it as
\ _d2_.
\
\ }doc

  zero_less_than_ __
  zero_branch_ __ d_plus_minus.end __
  dnegate_ __
$ constant d_plus_minus.end
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header abs_,"ABS"

\ doc{
\
\ abs  ( n -- u )
\
\ Leave the absolute constant _u_ of a number _n_.
\
\ }doc

  dup_ __ plus_minus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header dabs_,"DABS"

\ doc{
\
\ dabs  ( d -- ud )
\
\ Leave the absolute constant _ud_ of a double number _d_.
\
\ }doc

  dup_ __ d_plus_minus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header umax_,"UMAX"

\ doc{
\
\ umax  ( u1 u2 -- u1 | u2 )
\
\ }doc

  \ [Code from DZX-Forth.]

  de pop
  hl pop
  compare_de_hl_unsigned call
  jp max.1

\ ----------------------------------------------
  _code_header umin_,"UMIN"

\ doc{
\
\ umin  ( u1 u2 -- u1 | u2 )
\
\ }doc

  \ [Code from DZX-Forth.]

  de pop
  hl pop
  compare_de_hl_unsigned call
  jp max.2

\ ----------------------------------------------
  _code_header min_,"MIN"

  \ [Code from DZX-Forth.]

  de pop
  hl pop
  compare_de_hl_signed call
  jp max.2

\ ----------------------------------------------
  _code_header max_,"MAX"

  \ [Code from DZX-Forth.]

  de pop
$ constant max.de
  hl pop
  compare_de_hl_signed call
$ constant max.1
  ccf
$ constant max.2
  jp c,push_hl
  ex de,hl
  jp push_hl

\ ----------------------------------------------
  _colon_header m_star_,"M*"

\ doc{
\
\ m*  ( n1 n2 -- d )  \ fig-Forth
\
\ A mixed magnitude math operation which leaves the double
\ number signed product of two signed number.
\
\ }doc

  two_dup_ __
  xor_ __ to_r_ __
  abs_ __
  swap_ __ abs_ __ u_m_star_ __
  from_r_ __ d_plus_minus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header m_slash_,"M/"

\ doc{
\
\ m/  ( d n1 -- n2 n3 )  \ fig-Forth
\
\ A mixed magnitude math operator which leaves the signed
\ remainder _n2_ and signed quotient _n3_ from a double number
\ dividend and divisor _n1_.  The  remainder takes its sign from
\ the dividend.
\
\ }doc

  over_ __
  to_r_ __
  to_r_ __
  dabs_ __
  r_fetch_ __
  abs_ __
  u_slash_mod_ __
  from_r_ __
  r_fetch_ __
  xor_ __
  plus_minus_ __
  swap_ __
  from_r_ __
  plus_minus_ __
  swap_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header star_,"*"

  m_star_ __ drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header slash_mod_,"/MOD"

\ doc{
\
\ /mod  ( n1 n2 -- rem quot )  \ fig-Forth
\
\ Leave the remainder and signed quotient of _n1_/_n2_. The
\ remainder has the sign of the dividend.
\
\ }doc

  to_r_ __ s_to_d_ __
  from_r_ __ m_slash_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header slash_,"/"

  slash_mod_ __ nip_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header mod_,"MOD"

  slash_mod_ __ drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header star_slash_mod_,"*/MOD"

  to_r_ __ m_star_ __
  from_r_ __ m_slash_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header star_slash_,"*/"

  star_slash_mod_ __ nip_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header m_slash_mod_,"M/MOD"

  to_r_ __ zero_ __ r_fetch_ __
  u_slash_mod_ __
  from_r_ __ swap_ __
  to_r_ __ u_slash_mod_ __ from_r_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_line_,"(LINE)"

  to_r_ __
  c_slash_l_ __ b_slash_buf_ __ star_slash_mod_ __
  from_r_ __ b_slash_scr_ __ star_ __ plus_ __
  block_ __ plus_ __ c_slash_l_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header dot_line_,".LINE"

  paren_line_ __ minus_trailing_ __ type_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header message_,"MESSAGE"

\ doc{
\
\ message  ( n -- )
\
\ }doc

  warning_ __ fetch_ __
  zero_branch_ __ message.number_only __
  error_to_line_ __ msg_scr_ __ dot_line_ __ space_ __
  semicolon_s_ __

$ constant message.number_only
  paren_dot_quote_ __
  \ _string "MSG # "  \ XXX FIXME compiled with length 2!
  \ XXX TODO adapt
  db message.string_end-$-1
  db "MSG #"
$ constant message.string_end
  \ XXX TODO force decimal base
  dot_ __
  semicolon_s_ __


\ ----------------------------------------------
  _colon_header update_,"UPDATE"

\ doc{
\
\ update  ( -- )  \ ANS-Forth
\
\ Mark the most recently referenced block (pointed to by `prev`) as
\ altered. The block will subsequently be transferred automatically to
\ disk should its buffer be required for storage of a different block.
\
\ }doc

  \ XXX TODO move to the disk?

  disk_buffer_ __ fetch_ __
  lit_ __ 0x8000 __ or_ __
  disk_buffer_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header updated_question_,"UPDATED?"

\ doc{
\
\ updated?  ( -- f )
\
\ Is the current disk buffer marked as modified?
\
\ }doc

  buffer_id_ __ zero_less_than_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header stream_end_,"STREAM-END"

\ doc{
\
\ stream-end  ( ca -- )
\
\ Store the null word (a null character) at the given address,
\ surrounded by spaces. This marks the end of a input stream.
\
\ }doc

  s_lit_ __
  3 _ space_char _ 0 _ space_char _ \ string
  rot_ __ smove_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header empty_buffers_,"EMPTY-BUFFERS"

\ doc{
\
\ empty-buffers  ( -- )
\ 
\ Unassign all block buffers. Do not transfer the contents of
\ any updated block to mass storage.
\
\ }doc

  lit_ __ buffer_block_id_mask __ disk_buffer_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header buffer_data_,"BUFFER-DATA"

\ doc{
\
\ buffer-data  ( -- a )
\ 
\ First data address of the disk buffer.
\
\ }doc

  disk_buffer_ __ cell_plus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header buffer_id_,"BUFFER-ID"

\ doc{
\
\ buffer-id  ( -- x )
\ 
\ Id of the disk buffer.
\
\ }doc

  disk_buffer_ __ fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header block_number_,"BLOCK-NUMBER"

\ doc{
\
\ block-number  ( x -- n )
\ 
\ Convert the disk buffer id _x_ to its associated block _n_,
\ by removing the update bit.
\
\ }doc

  buffer_block_id_mask _literal
  and_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header buffer_block_,"BUFFER-BLOCK"

\ doc{
\
\ buffer-block  ( -- n )
\
\ Block number associated with the disk buffer.
\
\ }doc

  buffer_id_ __ block_number_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_buffer_,"(BUFFER)"

\ doc{
\
\ (buffer)  ( n -- )
\
\ If the contents of the disk buffer has been marked as updated,
\ write its block to the disk. Assign the block number _n_ to
\ the disk buffer.

\ ----
\ : (buffer)  ( n -- )
\   updated?  if    block-number write-buffer
\             else  drop
\             then  disk-buffer !  ;
\ ----
 
\ }doc

  updated_question_ __
  zero_branch_ __ free_buffer.not_updated __
  block_number_ __ write_buffer_ __
  branch_ __ free_buffer.end __
$ constant free_buffer.not_updated
  drop_ __
$ constant free_buffer.end
  disk_buffer_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header buffer_,"BUFFER"

\ doc{
\
\ buffer  ( n -- a )
\
\ Assign the block buffer to block _n_.   If the contents of the
\ buffer were marked as updated, it is written to the disk.  The
\ block _n_ is not read from the disk.  The address _a_ left on
\ stack is the first cell in the buffer for data storage.

\ ----
\ : buffer  ( n -- a )
\   dup buffer-block =  if    drop
\                       else  (buffer)
\                       then  buffer-data  ;
\ ----

\ }doc

  dup_ __ buffer_block_ __ equals_ __
  zero_branch_ __ buffer.not_equals __
  \ The requested block is the one already in the buffer.
  drop_ __
  branch_ __ buffer.end __
$ constant buffer.not_equals
  paren_buffer_ __
buffer.end:  
  buffer_data_ __ \ first cell of data in the buffer
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header block_,"BLOCK"

\ doc{
\
\ block  ( n -- a )

\ If the block _n_ is already in memory, leave the address _a_
\ of the first cell in the disk buffer for data storage.
\ 
\ If the block _n_ is not already in memory, transfer it from
\ disk to the buffer.  If the block occupying that buffer has
\ been marked as updated, rewrite it to disk before block _n_ is
\ read into the buffer.  Finally leave the address _a_ of the
\ first cell in the disk buffer for data storage.

\ ----
\ : block ( n --- a )
\   dup buffer-block =
\   if    drop
\   else  save-buffers  dup read-buffer  disk-buffer !
\   then  buffer-data  ;
\ ----

\ }doc

  dup_ __ buffer_block_ __ equals_ __
  zero_branch_ __ block.not_equals __
  drop_ __
  branch_ __ block.end __
$ constant block.not_equals
  save_buffers_ __ dup_ __ read_buffer_ __
  disk_buffer_ __ store_ __
$ constant block.end
  buffer_data_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header flip_,"FLIP"

\ doc{
\
\ flip  ( n1 -- n2 )
\
\   Exchange the low and high bytes within n1.
\
\ }doc

\ [Name taken from eForth. It's called `><` or `cswap` in other
\ Forth systems.]

  hl pop
  h a ld
  l h ld
  a l ld
  jp push_hl

\ ----------------------------------------------
  _colon_header block_to_sector_,"BLOCK>SECTOR"

\ doc{

\ block>sector  ( n1 -- n2 )

\ Convert the disk block _n1_ to the disk sector _n2_, in the
\ format required by G+DOS: The high byte of _n2_ is the track
\ (0..79 for side 0; 128..207 for side 1); the low byte of _n2_
\ is the sector (1..10).

\ ----
\ : block>sector  ( n1 -- n2 )
\   \ n2 (high byte) = track 0..79 for side 0, 128..207 for side 1
\   \    (low byte)  = sector 1..10
\   \ track0 = 0..79
\   \ track = 0..207
\   \ side = 0..1
\   dup 10 mod 1+    ( n1 sector )
\   swap dup 20 /    ( sector n1 track0 )
\   swap 10 / 1 and  ( sector track0 side )
\   128              ( sector track 128 )
\   \ * +          ( sector track ) \ XXX OLD for true=1
\   negate and or    ( sector track )  \ XXX NEW a bit faster, for true=-1
\   flip or  ;
\ ----
\
\ }doc

  dup_ __
  10 _literal
  mod_ __ one_plus_ __ swap_ __ dup_ __
  20 _literal
  slash_ __ swap_ __
  10 _literal
  slash_ __ one_ __ and_ __

true_flag 1 = [if] \ XXX OLD
  \ XXX TODO optimize with `7 lshift`?
  128 _literal
  star_ __ plus_ __
[else]
  negate_ __
  128 _literal
  and_ __ or_ __
[then]
  
  flip_ __ or_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header read_block_,"READ-BLOCK"

\ doc{
\
\ read-block  ( a n -- )
\
\ Read disk block _n_ to buffer _a_.
\
\ }doc

  0x44 _literal \ G+DOS command to read a disk sector
  transfer_block_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header read_buffer_,"READ-BUFFER"

\ doc{
\
\ read-buffer  ( n -- )
\
\ Read disk block _n_ to the disk buffer.
\
\ }doc

  buffer_data_ __ swap_ __ read_block_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header write_block_,"WRITE-BLOCK"

\ doc{
\
\ write-block  ( a n -- )
\
\ Write buffer _a_ to disk block _n_.
\
\ }doc

  0x45 _literal \ G+DOS command to write a disk sector
  transfer_block_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header write_buffer_,"WRITE-BUFFER"

\ doc{
\
\ write-buffer  ( n -- )
\
\ Write the disk buffer to disk block _n_.
\
\ }doc

  buffer_data_ __ swap_ __ write_block_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header transfer_block_,"TRANSFER-BLOCK"

\ doc{
\
\ transfer-block  ( a n b -- )
\
\ The disk read-write linkage.
\
\ a = source or destination block buffer
\ n = sequential number of the referenced disk block
\ b = G+DOS command to read or write a sector
\
\ }doc

  lit_ __ read_write_sector_command __ c_store_ __
  block_to_sector_ __
  paren_transfer_block_ __
  semicolon_s_ __

$ constant paren_transfer_block_
  \ Headerless word with the low level code of `R/W`.
  paren_transfer_block_pfa __ \ code field
  
  \ ( a sector -- )
  \ sector (high byte) = track 0..79, +128 if side 1
  \        (low byte)   = sector 1..10
$ constant paren_transfer_block_pfa
  de pop \ d = track 0..79, +128 if side 1
         \ e = sector 1..10
  pop ix \ address
  bc push \ save the Forth IP
  ld a,2 \ drive ; XXX TMP
  rst 8 \ G+DOS hook
$ constant read_write_sector_command
  \ G+DOS command already patched:
  0x44 _ \ 0x44 = read ; 0x45 = write
  bc pop \ restore the Forth IP
  ld ix,next
  jpnext

\ ----------------------------------------------
  _colon_header save_buffers_,"SAVE-BUFFERS"

\ doc{
\
\ save-buffers  ( -- )  \ ANS Forth
\
\ If the disk buffer has been modified, transfer its contents to
\ disk and mark it as unmodified.
\
\ ----
\ : save-buffers ( -- )
\   updated? 0= ?exit \ exit if not updated
\   buffer-block dup write-buffer  disk-buffer !  ;
\ ----
\
\ }doc

  updated_question_ __ zero_equals_ __ \ not updated?
  question_exit_ __ \ exit if not updated
  \ Updated
  buffer_block_ __ dup_ __ write_buffer_ __
  disk_buffer_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header flush_,"FLUSH"

  \ XXX TODO -- move to the disk?

  save_buffers_ __ empty_buffers_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header paren_load_,"(LOAD)"

\ doc{
\
\ (load)  ( i*x u -- j*x )
\
\ Store _u_ in `blk` (thus making block _u_ the input source and
\ setting the input buffer to encompass its contents), set `>in`
\ to zero, and interpret.  Other stack effects are due to the
\ words loaded.
\
\ An error is issued if _u_ is zero.
\
\ ----
\ : (load)  ( i*x u -- j*x )
\   dup 0= 9 ?error
\   b/scr * blk !  >in off  interpret  ;
\ ----
\
\ }doc

  dup_ __ zero_equals_ __
  _question_error error.loading_from_screen_0
  b_slash_scr_ __ star_ __ blk_ __ store_ __
  to_in_ __ off_ __ interpret_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header continued_,"CONTINUED"

\ doc{
\
\ continued  ( i*x u -- j*x )  \ Forth-79 (uncontrolled word
\ definition from the Reference Word Set)
\
\ Store _u_ in `blk` (thus making block _u_ the input source and
\ setting the input buffer to encompass its contents), set `>in`
\ to zero, and interpret.  Other stack effects are due to the
\ words loaded.
\
\ ----
\ : continued  ( -- )
\   ?loading (load)  ;
\ ----
\
\ }doc

  question_loading_ __ paren_load_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header load_,"LOAD"

\ doc{
\
\ load  ( u -- )
\
\ Save the current input-source specification. Store _u_ in
\ `blk` (thus making block _u_ the input source and setting the
\ input buffer to encompass its contents), set `>in` to zero,
\ and interpret. When the parse area is exhausted, restore the
\ prior input source specification. Other stack effects are due
\ to the words loaded.
\
\ An error is issued if _u_ is zero.
\
\ ----
\ : load  ( n -- )
\   blk @ >r  >in @ >r
\   (load)
\   r> >in !  r> blk !  ;
\ ----
\
\ }doc

  blk_ __ fetch_ __ to_r_ __
  to_in_ __ fetch_ __ to_r_ __
  paren_load_ __
  from_r_ __ to_in_ __ store_ __
  from_r_ __ blk_ __ store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header next_screen_,"-->",immediate

\ doc{
\
\ -->  ( -- )  \ "next-screen"
\
\ Continue interpretation with the next disk screen.
\
\ ----
\ : -->  ( -- )
\   ?loading  >in off
\   b/scr blk @ over mod - blk +!  \ immediate
\ ----
\
\ }doc

  question_loading_ __
  to_in_ __ off_ __
  dw b_slash_scr_,blk_,fetch_,over_,mod_,minus_
  blk_ __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header defined_,"DEFINED"

\ doc{
\
\ defined  ( "name" -- ca 0 | cfa 1 | cfa -1 )
\
\ }doc

  \ XXX TODO -- adapt to the new parsing method

1 [if] \ XXX OLD

  b_l_ __ word_ __ \ ( ca2 )

[else] \ XXX NEW

  \ XXX FIXME -- crash somewhere in `query`

  parse_name_ __  \ ( ca1 len1 )
  \ dw cr_,dot_s_,lit_,1,border_,key_,drop_ \ XXX INFORMER
  save_counted_string_ __ \ ( ca2 )
  \ dw cr_,dot_s_,lit_,2,border_,key_,drop_ \ XXX INFORMER

[then]

  dup_ __ count_ __ uppers_ __  \ uppercase ( ca2 )
  \ dw cr_,dot_s_,lit_,3,border_,key_,drop_ \ XXX INFORMER
  find_ __
  \ dw cr_,dot_s_,lit_,4,border_,key_,drop_ \ XXX INFORMER

  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_defined_,"?DEFINED"

\ doc{
\
\ ?defined  ( f -- )
\
\ }doc

  \ [Code from DZX-Forth.]

  zero_equals_ __
  _question_error error.not_found
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header bracket_defined_,"[DEFINED]",immediate

\ doc{
\
\ [defined]  ( "name" -- wf )
\
\ }doc

  defined_ __ nip_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header bracket_undefined_,"[UNDEFINED]",immediate

\ doc{
\
\ [undefined]  ( "name" -- wf )
\
\ }doc

  bracket_defined_ __ zero_equals_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header tick_,"'"

\ doc{
\
\ '  ( "name" -- cfa )
\
\ }doc

  defined_ __ question_defined_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header bracket_tick_,"[']",immediate

\ doc{
\
\ [']  ( "name" -- cfa )
\
\ }doc

  tick_ __ literal_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header begin_,"BEGIN",immediate

\ doc{
\
\ begin  ( compilation: -- a n )
\
\ At compile time `begin` leaves the dictionary address on
\ stack with an error checking number _n_.  It does not compile
\ anything to the dictionary.
\
\ }doc

  question_comp_ __ \ error if not compiling
  backward_mark_ __ \ address to compute the backward branch
fig_compiler_security? [if]
  one_ __ \ error checking number
[then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header then_,"THEN",immediate

  question_comp_ __ \ error if not compiling
fig_compiler_security? [if]
  two_ __ question_pairs_ __ \ check for nesting error
[then]
  forward_resolve_ __
  semicolon_s_ __

\ ----------------------------------------------
  \ _colon_header question_do_,"?DO",immediate

  \ XXX TODO

  \ compile_ __ paren_question_do_ __
  \ branch_ __ do.common __

\ ----------------------------------------------
  _colon_header do_,"DO",immediate

  compile_ __ paren_do_ __
$ constant do.common
  backward_mark_ __
fig_compiler_security? [if]
  3 _literal \ error checking number
[then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header loop_,"LOOP",immediate

fig_compiler_security? [if]
  3 _literal \ error checking number
  question_pairs_ __
[then]
  compile_ __ paren_loop_ __
  backward_resolve_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header plus_loop_,"+LOOP",immediate

fig_compiler_security? [if]
  3 _literal \ error checking number
  question_pairs_ __
[then]
  compile_ __ paren_plus_loop_ __
  backward_resolve_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header until_,"UNTIL",immediate

fig_compiler_security? [if]
  one_ __ question_pairs_ __
[then]
  compile_ __ zero_branch_ __
  backward_resolve_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header again_,"AGAIN",immediate

\ doc{
\
\ again  ( compilation: a n -- )
\
\ End of an infinite loop.  Compile an unconditional jump
\ instruction to branch backward to _a_.
\
\ }doc

fig_compiler_security? [if]
  one_ __ question_pairs_ __ \ check n for error
[then]
  compile_ __ branch_ __
  backward_resolve_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header repeat_,"REPEAT",immediate

\ doc{
\
\ repeat  ( compilation: a1 n1 a2 n2 -- )
\
\ a1 = address of `begin` to branch to
\ n1 = `begin` check number
\ a2 = address of the branch of `while` to resolve
\ n2 = `while` check number
\
\ Compile `branch` to jump back to `begin`.  Resolve also  the
\ branching offset required by `while`.
\
\ }doc

  two_to_r_ __
  again_ __ \ unconditional branch back to `begin`
  two_from_r_ __
  \ two_minus_ __ \ restore 2 to be checked by `then` ; XXX OLD -- unnecessary
  \ XXX TODO why unncessary?
  then_ __ \ resolve the forward branching needed by `while`
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header if_,"IF",immediate

  compile_ __ zero_branch_ __
$ constant if.do
  forward_mark_ __
fig_compiler_security? [if]
  two_ __ \ error checking number
[then]
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header unless_,"UNLESS",immediate

  \ Equivalent to `0= if`, but faster.

  compile_ __ question_branch_ __
  branch_ __ if.do __

  \ XXX TODO move to the disk:

\ : unless  ( f -- )  postpone ?branch >mark 2  \ immediate
\ \ Alternative: when compiler security is removed:
\ : unless  ( f -- )  postpone ?branch >mark  \ immediate

\ ----------------------------------------------
  _colon_header ahead_,"AHEAD",immediate

  compile_ __ branch_ __
  forward_mark_ __

\ ----------------------------------------------
  _colon_header else_,"ELSE",immediate

  \ XXX FIXME Pasmo bug?
  \ The system crashes when conditional compilation
  \ is used here, in any combination. Very strange.

fig_compiler_security? [if]

  two_ __ \ error checking number
  question_pairs_ __
  compile_ __ branch_ __
  forward_mark_ __
  swap_ __
  two_ __ \ error checking number
  then_ __
  two_ __ \ error checking number
  semicolon_s_ __

[else]

  compile_ __ branch_ __
  forward_mark_ __
  then_ __
  semicolon_s_ __

[then]

\ ----------------------------------------------
  _colon_header while_,"WHILE",immediate

  if_ __
  \ two_plus_ __ \ leave 4 to be checked by `repeat` ; XXX OLD -- unnecessary
  \ XXX TODO 2015-08-13: why unnecessary? why fig-Forth does not use `swap`?
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header spaces_,"SPACES"

  b_l_ __ emits_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header emits_,"EMITS"

  \ emits  ( u c -- )

  \ XXX TODO use `?do` or `for` when available
  swap_ __ zero_ __ max_ __ question_dup_ __
  zero_branch_ __ emits_.end __
  zero_ __
  paren_do_ __
$ constant emits_.do
  dup_ __ emit_ __
  paren_loop_ __ emits_.do __
$ constant emits_.end
  drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header less_hash_,"<#"

  pad_ __
  hld_ __
  store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header hash_greater_,"#>"

  drop_ __
  drop_ __
  hld_ __ fetch_ __
  pad_ __
  over_ __
  minus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header sign_,"SIGN"

\ doc{

\ sign  ( n  d  ---  d ) \ fig-Forth

\ Stores an ascii "-" sign just before a converted numeric
\ output string in the text output buffer when _n_ is negative.
\ _n_ is discarded but double number _d_ is maintained. Must be
\ used between `<#` and `#>`.

\ }doc

  \ XXX TODO convert to ANS Forth

  rot_ __ zero_less_than_ __
  zero_branch_ __ sign.end __
  '-' _literal
  hold_ __
$ constant sign.end
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header hash_,"#"

\ doc{
\
\ #  ( d1 -- d2 )
\
\ Divide _d1_ by current base.  The remainder is converted to
\ an ASCII character and appended to the output text string.
\ The quotient _d2_ is left on stack.
\
\ }doc


  base_ __ fetch_ __
  m_slash_mod_ __ \  ( remainder dquotient )
  rot_ __ \ ( dquotient remainder )
  0x09 _literal
  over_ __ less_than_ __ \ remainder<9?
  zero_branch_ __ hash.digit __
  \ remainder<9
  0x07 _literal
  plus_ __ \ make it an alphabet
$ constant hash.digit
  \ Form the ASCII representation of a digit:
  \ "0" to "9" and "A" to "F" (or above).
  '0' _literal
  plus_ __
  hold_ __ \ put the digit into `pad` in a reversed order.
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header hash_s_,"#S"

\ doc{
\
\ #S  ( d1 -- d2 )
\
\ }doc

$ constant hash_s.begin
  hash_ __ two_dup_ __ or_ __
  question_branch_ __ hash_s.begin __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header d_dot_r_,"D.R"

\ doc{
\
\ d.r  ( d n -- )
\
\ Print a signed double number _d_ right justified in a field of
\ _n_ characters.
\
\ }doc

  to_r_ __ \ save n
  \ Save the high order part of d under d,
  \ to be used by `sign` to add a "-" sign to a negative number:
  swap_ __ over_ __
  dabs_ __ \ convert d to its absolute constant
  \ Convert the absolute constant to ASCII text with proper sign:
  less_hash_ __
  hash_s_ __
  sign_ __
  hash_greater_ __
  from_r_ __ \ retrieve n
  over_ __ minus_ __ spaces_ __ \ fill the output field with preceding blanks
  type_ __ \ type out the number
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header dot_r_,".R"

  to_r_ __ s_to_d_ __ from_r_ __ d_dot_r_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header d_dot_,"D."

\ doc{
\
\ d.  ( d -- )
\
\ Print signed double integer _d_ according to current base,
\ followed by only one blank.
\
\ }doc

  zero_ __ d_dot_r_ __ space_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header dot_,"."

\ doc{
\
\ .  ( n -- )
\
\ Print signed integer _n_ according to current base, followed
\ by only one blank.
\
\ }doc

  s_to_d_ __ d_dot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_,"?"

  fetch_ __ dot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header u_dot_,"U."

  zero_ __ d_dot_ __
  semicolon_s_ __

\ ----------------------------------------------

1 [if]

  _colon_header dot_s_,".S"

  \ XXX TMP -- only during the development, then remove
  \ It is already on the disk.

  depth_ __ dup_ __ s_to_d_ __ less_hash_ __
  '>' _literal
  hold_ __ hash_s_ __
  '<' _literal
  hold_ __ hash_greater_ __ type_ __ space_ __
  zero_branch_ __ dot_s.end __

  dw sp_fetch_,two_minus_,sp0_,fetch_,two_minus_
  paren_do_ __
$ constant dot_s.do
  i_ __ fetch_ __ u_dot_ __ \ XXX TMP `u.`
  dw lit_,-2
  paren_plus_loop_ __ dot_s.do __
$ constant dot_s.end
  semicolon_s_ __

[then]

\ ----------------------------------------------
  _code_header colors0_,"COLORS0"

\ doc{
\
\ colors0  ( -- )
\ 
\ Set the screen colors to the default values.
\
\ }doc

  \ Set the colors and their masks.

  ld hl,(default_color_attribute)
  \ l = 128*flash + 64*bright + 8*paper + ink
  \ h = mask
  ld (sys_attr_p),hl \ permanent
  ld (sys_attr_t),hl \ temporary

  \ Set the system variable that holds the attributes of the
  \ lower part of the screen.  It is needed only because G+DOS
  \ by default changes the border color during disk operations,
  \ and at the end restores it with the constant of this system
  \ variable.

  l a ld
  ld (sys_bordcr),a \ lower screen colors

  \ Set the border color to the paper color.

  \ a = 128*flash + 64*bright + 8*paper + ink
  a and \ cy=0
  rra
  rra
  rra \ a = paper
  out (border_port),a \ set the border color

  jpnext

\ ----------------------------------------------
  _code_header home_,"HOME"

\ doc{
\
\ home  ( -- )
\
\ Reset the cursor position to the upper left corner (column 0,
\ row 0).
\
\ }doc

  ld hl,0x1821 \ 0x18 = 24 - row
               \ 0x21 = 33 - column
  ld (sys_s_posn),hl
  jpnext

\ ----------------------------------------------
  _code_header cls_,"CLS"


\ doc{
\
\ cls  ( -- )
\
\ Clear the screen with the current colors and reset the cursor
\ position to the upper left corner (column 0, row 0).
\
\ }doc

\ Note: The ROM routines that clear the screen are slow and do
\ many unnecessary BASIC-related things. This code simply clears
\ the screen.

  \ XXX TODO compare size with Forth

  exx \ save the Forth IP
  \ Erase the bitmap.
  ld hl,sys_screen
  ld de,sys_screen+1
  ld bc,sys_screen_bitmap_size
  ld (hl),0
  ldir
  \ Color with the permanent attributes.
  ld hl,sys_screen_attributes
  ld de,sys_screen_attributes+1
  ld bc,sys_screen_attributes_size
  ld a,(sys_attr_p)
  ld (hl),a
  ldir
  exx \ restore the Forth IP
  jp home_pfa \ continue at `home`

\ ----------------------------------------------
  _colon_header page_,"PAGE"

\ doc{
\
\ page  ( -- )  \ ANS Forth
\
\ Move to another page for output.  On a terminal, `page` clears
\ the screen and resets the cursor position to the upper left
\ corner. On a printer, `page` performs a form feed.
\
\ }doc

  \ XXX TODO printer support

  cls_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header bye_,"BYE"

  ld (iy+sys_df_sz_offset),0x02 \ restore lines of the lower screen
system_stack_pointer: equ $+1
  ld sp,0 \ restore the system stack
latin1_charset_in_bank? [if]
  \ Restore the default charset:
  ld hl,15360
  ld (sys_chars),hl
[then]
  \ Exit to BASIC:
  rst 0x08
  0x08 _ \ "STOP" BASIC error

\ ----------------------------------------------
  _code_header two_drop_,"2DROP"

  hl pop
  hl pop
  jpnext

\ ----------------------------------------------
  _code_header two_swap_,"2SWAP"

  \ [Code from DZX-Forth.]

  hl pop
  de pop
  ex (sp),hl
  hl push
  ld hl,5
  add hl,sp
  ld a,(hl)
  ld (hl),d
  a d ld
  hl decp
  ld a,(hl)
  ld (hl),e
  a e ld
  hl pop
  jp push_hlde

\ ----------------------------------------------
  _colon_header unused_,"UNUSED"

  \ XXX TMP
  zero_ __ here_ __ minus_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header where_,"WHERE"

  \ XXX TODO -- remove; already copied to the disk

  error_pos_ __ two_fetch_ __ \ XXX NEW
  dup_ __
  question_branch_ __ where.do_it __
  two_drop_ __
  semicolon_s_ __

$ constant where.do_it
  dup_ __ b_slash_scr_ __ slash_ __
  paren_dot_quote_ __
  \ _string "Scr # " \ XXX FIXME compiled with length 2!
  \ XXX TODO adapt
  db where.string_1_end-$-1
  db "Scr #"
$ constant where.string_1_end
  decimal_ __ dot_ __
  dw swap_,c_slash_l_,slash_mod_,c_slash_l_,star_
  rot_ __ block_ __ plus_ __
  c_slash_l_ __ cr_ __ type_ __ cr_ __
  here_ __ c_fetch_ __ minus_ __ spaces_ __
  '^' _literal
  emit_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header at_xy,"AT-XY"

\ doc{

\ at-xy ( col line -- )  \ ANS Forth

\ Warning: The system will crash if the coordinates are out of screen.
\ For the sake of speed, no check is done.  A wrapper secure word can
\ be written if needed.

\ }doc

\ [Code adapted from Spectrum Forth-83.]

  dup_ __
  23 _literal
  not_equals_ __ \ not the last line?
  zero_branch_ __ at_pfa.last_line __
  \ not the last line
  dw lit_,22,paren_emit_,paren_emit_,paren_emit_
  semicolon_s_ __ \ XXX TODO exit_

$ constant at_pfa.last_line
  dw one_minus_,dup_,paren_emit_,paren_emit_,zero_,paren_emit_
  cr_ __
  dup_ __
  lit_ __ sys_df_cc __ \ address in display file of print position
  plus_store_ __
  33 _literal
  swap_ __
  minus_ __
  lit_ __ sys_s_posn __ \ 33 minus column number for print position
  c_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header border_,"BORDER"

  hl pop
  l a ld
  out (border_port),a

  \ The system variable that holds the attributes of the lower
  \ part of the screen, unnecessary in Solo Forth, must be
  \ updated.  The reason is G+DOS, after disk operations that
  \ make the border change, restores the border color with the
  \ constant of this system variable.  We use the border color as
  \ paper and set a a contrast ink (black or white), to make
  \ sure the lower part of the screen is usable after returning
  \ to BASIC.

  \ XXX TODO move the contrast ink calculation to `bye` or
  \ simply remove it:

  cp 4 \ cy = dark color (0..3)?
  ld a,7 \ white ink
  jr c,border.end
  a xor \ black ink

$ constant border.end
  \ Note: slower than shifting the register, but saves three bytes.
  add hl,hl
  add hl,hl
  add hl,hl \ l = paper (bits 3..5)
  l or \ combine with ink
  ld (sys_bordcr),a
  jpnext

\ ----------------------------------------------
  _code_header overwrite_,"OVERWRITE"

  ld a,over_char
  jp color

\ ----------------------------------------------
  _code_header flash_,"FLASH"

  ld a,flash_char
  jp color

\ ----------------------------------------------
  _code_header inverse_,"INVERSE"

  ld a,inverse_char
  jp color

\ ----------------------------------------------
  _code_header bright_,"BRIGHT"

  ld a,bright_char
  jp color

\ ----------------------------------------------
  _code_header paper_,"PAPER"

  ld a,paper_char
  jp color

\ ----------------------------------------------
  _code_header ink_,"INK"

  ld a,ink_char

$ constant color
  \ Set a color attribute (ink, paper, bright, flash, inverse or
  \ overwrite).
  \ Input:
  \   a = attribute control char
  \   (tos) = color attribute constant
  rst 0x10
  hl pop
  l a ld
  rst 0x10
  rom_set_permanent_colors_0x1CAD call
  jpnext

\ ----------------------------------------------
  _code_header emitted_,"EMITTED"

  \ [Code adapted and modified from the ZX Spectrum ROM routine
  \ S-SCRN$-S at 0x2535.]

\ doc{
\
\ emitted  ( col row -- n | 0 )
\
\ Return the ordinal number _n_ (first is 1) of the character
\ printed at the given screen coordinates, or 0 if no character
\ can be recognized on that position of the screen.
\
\ This word must be configured by `emitted-charset` and
\ `#emitted-chars`, that set the address of the first character
\ and the number of characters to compare with. By default the
\ printable ASCII chars of the ROM charset are used.
\
\ The result _n_ is the ordinal number (first is 1) of the
\ recognized char in the specified charset. Example: with the
\ default configuration, a recognized space char would return 1;
\ a "!" char, 2; a "A", 34...
\
\ This word is meant to be used with user defined graphics.
\
\ }doc

  \ XXX TODO improve the result
  ;
  \ XXX TODO move to the disk
  ;
  \ XXX TODO rename?: `ocr`, `recognized`, `on-xy`, `xy-char`?
  \ The reasen is name clash with the fig-Forth `out` counter,
  \ that was going to be called `emitted` or `#emitted`.

  de pop \ row
  hl pop \ col
  bc push \ save the Forth IP
  l b ld \ column
  e c ld \ row
  ld hl,(emitted_charset_pfa) \ address of first printable char in the charset
  c a ld  \ row
  rrca
  rrca
  rrca \ multiply by 0x20
  and  %11100000
  b xor \ combine with column (0x00..0x1F)
  a e ld \ low byte of top row = 0x20 * (line mod 8) + column
  c a ld  \ row is copied to a again
  and  0x18
  xor  0x40
  a d ld \ high byte of top row = 64 + 8*int (line/8)
  \ de = screen address
  ld a,(hash_emitted_chars_pfa) \ number of chars in the charset
  a b ld

$ constant emitted.do
  bc push  \ save the characters count
  de push  \ save the screen pointer
  hl push  \ save the character set pointer (bitmap start)
  ld  a,(de)  \ get first scan of screen character
  (hl) xor  \ match with scan from character set
  jp z,emitted.match  \ jump if direct match found
  \ if inverse, a=0xFF
  a inc  \ inverse? (if inverse, a=0)
  jp  nz,emitted.next_char  \ jump if inverse match not found
  \ inverse match
  a dec  \ restore 0xFF
$ constant emitted.match
  a c ld  \ inverse mask (0x00 or 0xFF)
  ld  b,0x07  \ count 7 more character rows
$ constant emitted.scans
  d inc  \ next screen scan (add 0x100)
  hl incp  \ next bitmap address
  ld  a,(de)  \ screen scan
  (hl) xor  \ will give 0x00 or 0xFF (inverse)
  c xor  \ inverse mask to include the inverse status
  jp  nz,emitted.next_char  \ jump if no match
  djnz  emitted.scans  \ jump back till all scans done

  \ character match
  bc pop  \ discard character set pointer
  bc pop  \ discard screen pointer
  bc pop  \ final count
  ld a,(hash_emitted_chars_pfa) \ number of chars in the charset
  sub  b \ ordinal number of the matched character (1 is the first)
  a l ld
  jp emitted.end

$ constant emitted.next_char
  hl pop  \ restore character set pointer
  ld  de,0x0008  \ move it on 8 bytes
  add  hl,de  \ to the next character in the set
  de pop  \ restore the screen pointer
  bc pop  \ restore the counter
  djnz  emitted.do  \ loop back for the 96 characters
  \ no match
  b l ld \ zero

$ constant emitted.end
  bc pop \ restore the Forth IP
  ld h,0
  jp push_hl

\ ----------------------------------------------
  _variable_header emitted_charset_,"EMITTED-CHARSET"

\ doc{
\
\ emitted-charset  ( -- a )
\
\ Variable that holds the address of the first printable char in
\ the charset used by `emitted`. By default it contains 0x3D00, the
\ address of the space char in the ROM charset.
\
\ }doc

  0x3D00 __ \ address of the space in the ROM charset

\ ----------------------------------------------
  _variable_header hash_emitted_chars_,"#EMITTED-CHARS"

\ doc{
\
\ #emitted-charset  ( -- a )
\
\ Variable that holds the number of printable chars in the
\ charset used by `emitted`. By default it contais 0x5F, the
\ number of printable ASCII chars in the ROM charset.
\
\ }doc

  0x5F __ \ printable ASCII chars in the ROM charset

\ ----------------------------------------------
  _code_header j_,"J"

\ doc{
\
\ j  ( -- x ) ( R: loop-sys1 loop-sys2 -- loop-sys1 loop-sys2 ) \ ANS Forth
\
\ Return a copy of the next-outer loop index.
\
\ }doc

  ld hl,(return_stack_pointer)
  ld de,cell*2
  add hl,de
  jp fetch.hl

\ ----------------------------------------------
  _colon_header two_constant_,"2CONSTANT"

  two_variable_ __
  paren_semicolon_code_ __
$ constant do_two_constant
  de incp    \ de=pfa
  ex de,hl  \ hl=pfa
  jp two_fetch.hl

\ ----------------------------------------------
  _colon_header two_variable_,"2VARIABLE"

\ doc{
\
\ 2variable ( "name" -- )  \ ANS Forth
\
\ Parse _name_.  Create a definition for _name_ with the
\ execution semantics defined below. Reserve two consecutive
\ cells of data space.
\
\    _name_ is referred to as a two-variable.
\
\          name Execution: ( -- a )
\
\    _a_ is the address of the first (lowes address) cell of two
\    consecutive cells. A program is responsible for
\    initializing the contents.
\
\ }doc

  create_ __
  lit_ __ 2 cells __ allot_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header u_dot_r_,"U.R"

  to_r_ __ zero_ __ from_r_ __ d_dot_r_ __
  semicolon_s_ __

\ ----------------------------------------------
  _code_header two_over_,"2OVER"

\ doc{
\
\ 2over  ( d1 d2 -- d1 d2 d1 )
\
\ }doc

  ld hl,4
  add hl,sp
  jp two_fetch.hl

1 [if] \ fig_exit?

\ ----------------------------------------------
  _colon_header exit_,"EXIT"

\ doc{
\
\ exit  ( -- ) ( R: a -- )  \ ANS Forth
\
\ Return control to the calling definition, specified by the
\ address on the return stack.
\
\ Before executing `exit` within a do-loop, a program shall
\ discard the loop-control parameters by executing `unloop`.
\
\ }doc

\ XXX TODO combine this `exit` with `;s`?

  r_drop_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_exit_,"?EXIT"

\ doc{
\
\ ?exit  ( f -- ) ( R: a | -- a | )
\
\ If _f_ is non-zero, return control to the calling definition,
\ specified by the address on the return stack.
\
\ `?exit` is not intended to be used within a do-loop. Use `if
\ unloop exit then` instead.
\
\ }doc

  question_branch_ __ exit_pfa __
  semicolon_s_ __

[else]

\ ----------------------------------------------
  _code_header question_exit_,"?EXIT"

\ doc{
\
\ ?exit  ( f -- ) ( R: a | -- a | )
\
\ If _f_ is non-zero, return control to the calling definition,
\ specified by the address on the return stack.
\
\ `?exit` is not intended to be used within a do-loop. Use `if
\ unloop exit then` instead.
\
\ }doc

  hl pop
  a h ld
  l or
  jp nz,exit_pfa
  jpnext

[then]

\ ----------------------------------------------
  _colon_header char_,"CHAR"

  parse_name_ __ drop_ __ c_fetch_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header bracket_char_,"[CHAR]",immediate

  char_ __ literal_ __
  semicolon_s_ __

\ ----------------------------------------------
  \ _colon_header s_quote_,"S\"",immediate \ XXX FIXME as error
  _colon_header s_quote_,"S\x22",immediate

  \ : s"  ( compilation: "text<">" -- ) ( run-time:  -- ca len )
  \  [char] " (s)  \ immediate

  '"' _literal
  paren_s_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_next_screen_,"?-->",immediate

  zero_branch_ __ question_next_screen.end __
  next_screen_ __
$ constant question_next_screen.end
  semicolon_s_ __

  \ XXX TODO a good place to use `??` instead of a branch:
\  question_question_ __ next_screen_ __
\  semicolon_s_ __

\ ----------------------------------------------
  _colon_header question_backslash_,"?\\",immediate

\ doc{
\
\ ?\  ( f "ccc<eol> -- )
\
\ If _f_ is not false, parse and discard the rest of the parse
\ area. This word is used for conditional compilation.
\
\ }doc

  zero_branch_ __ question_backslash.end __
  backslash_ __
$ constant question_backslash.end
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header backslash_,"\\",immediate

\ doc{
\
\ \  ( -- )
\ 
\ Parse and discard the rest of the parse area.
\
\ }doc

  to_in_ __ fetch_ __ c_slash_l_ __ mod_ __
  c_slash_l_ __ swap_ __ minus_ __
  to_in_ __ plus_store_ __
  semicolon_s_ __

\ ----------------------------------------------
  _colon_header dot_paren_,".(",immediate

\ doc{
\
\ .(  ( 'text<paren>' -- )  \ immediate
\
\ }doc

  ')' _literal
  parse_ __ type_ __
  semicolon_s_ __

  .data
dot_paren_nfa constant latest_nfa_in_forth_voc
  .text

$ constant dictionary_pointer_after_cold

\ ==============================================================
\ Name and link fields

$ constant move_name_fields_to_memory_bank

  \ Move the name fields, assembled in ordinary memory, to the
  \ names bank. This routine is needed only once, therefore its
  \ is call patched with `noop` at the end; the routine itself
  \ will be overwritten by the Forth dictionary.

  \ The whole screen is used as intermediate buffer for copying
  \ the data.

  ld hl,names_bank_address \ origin
  ld de,sys_screen \ destination
  ld bc,sys_screen_size \ count
  ldir \ copy the data to the screen
  \ _z80_border_wait 1 \ XXX INFORMER
  ld e,names_bank
  bank.e call
  ld hl,sys_screen \ origin
  ld de,names_bank_address \ destination
  ld bc,sys_screen_size \ count
  ldir \ copy the name fields to the bank
  \ _z80_border_wait 2 \ XXX INFORMER
latin1_charset_in_bank? [if]
  ld hl,sys_screen+sys_screen_size-charset_size \ origin
  ld de,charset_address \ destination
  ld bc,charset_size \ count
  ldir \ copy the charset to the bank
[then]
  ld e,default_bank
  bank.e call

  \ Erase the default bank (not necessary) \ XXX OLD
  \ ld hl,names_bank_address \ the first byte is 0
  \ ld de,names_bank_address+1
  \ ld bc,sys_screen
  \ ldir

  \ Remove the to call this routine:
  ld hl,only_first_cold \ address of the to call this routine
  ld (hl),0 \ nop
  hl incp
  ld (hl),0 \ nop
  hl incp
  ld (hl),0 \ nop
  \ _z80_border_wait 3 \ XXX INFORMER
  ret

\ ==============================================================
\ Data section

  .data

  0 _ \ fake length byte, needed by the algorithm used in `cfa>nfa`

$ constant data_start

\ ==============================================================
\ Character set

\ XXX OLD

latin1_charset_in_bank? [if]

  .org names_bank_address+sys_screen_size-charset_size

  incbin solo_forth.charset.bin

[then]

\ ==============================================================
\ End

.end

\ ==============================================================
\ Debug tool ,s

  \ dw lit_,2,border_,key_,drop_,lit_,7,border_ \ XXX INFORMER
  \ dw lit_,0,border_,key_,drop_ \ XXX INFORMER
  \ dw lit_,1,border_,key_,drop_ \ XXX INFORMER
  \ dw lit_,2,border_,key_,drop_ \ XXX INFORMER
  \ dw lit_,4,border_,key_,drop_ \ XXX INFORMER
  \ dw lit_,5,border_,key_,drop_ \ XXX INFORMER
  \ dw lit_,6,border_,key_,drop_ \ XXX INFORMER
  \ dw lit_,7,border_,key_,drop_ \ XXX INFORMER

  \ dw two_dup_,two_,ink_,type_,zero_,ink_ \ XXX INFORMER

  \ XXX TMP commented out for debugging


\ ==============================================================
\ Development notes
\
\ 2015-06-25:
\
\ Number  Times compiled (not including error numbers)
\ 0       20
\ 1       11
\ 2       11
\ 3       6

\ vim: ft=gforth
