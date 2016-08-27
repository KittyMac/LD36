using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MarkdownDeep;
using System.Text;

public class StoryEngine {

	public class Dialog {
		public string text;
		public string room;
	}

	public class Room {
		public string hashtag;
		public string character;
		public string text;

		public List<Dialog> responses = new List<Dialog>();

		public void Print() {
			StringBuilder sb = new StringBuilder ();
			foreach (Dialog d in responses) {
				sb.AppendFormat ("{0} -> {1}\n", d.text, d.room);
			}
			Debug.Log (string.Format ("{0} : {1} : {2}\n{3}\n", hashtag, character, text, sb.ToString ()));
		}
	}

	public Dictionary<string,Room> AllRooms = new Dictionary<string, Room>();



	// ************************ UTILITY *********************************

	public Room GetRoom(string name) {
		if (AllRooms.ContainsKey (name)) {
			return AllRooms [name];
		}

		Debug.Log ("room does not exist: " + name);
		return null;
	}

	public void PrintAllRooms () {
		foreach (Room r in AllRooms.Values) {
			r.Print ();
		}
	}


	// *********************************************************************************




	// ************************ LOAD STORY FROM MARKDOWN *********************************

	public void Clear() {
		AllRooms.Clear ();
	}

	public void LoadStoryFromMarkdown(string pathToMarkdown) {

		string markdownString = PlanetUnityResourceCache.GetTextFile (pathToMarkdown);

		// run the parser on the content
		Markdown md = new Markdown ();

		md.ExtraMode = true;

		StringBuilder currentString = new StringBuilder ();
		Block currentBlock = null;
		Stack<BlockType> listStack = new Stack<BlockType>();

		Room currentRoom = null;
		Dialog currentDialog = null;

		Action CommitMarkdownBlock = () => {

			if (currentBlock == null) {
				return;
			}

			if (currentBlock.blockType == BlockType.p) {
				//mdStyle.Create_P(container, currentString.ToString());
			}

			if (currentBlock.blockType == BlockType.h1) {
				if(currentRoom != null){
					AllRooms[currentRoom.hashtag] = currentRoom;
				}
				currentRoom = new Room();
				currentRoom.hashtag = "#" + currentString.ToString();
			}


			if (currentBlock.blockType == BlockType.ol_end) {
				currentRoom.responses.RemoveAt(currentRoom.responses.Count-1);
			}

			if (currentBlock.blockType == BlockType.ol ||
				currentBlock.blockType == BlockType.ol_li) {
				if(currentRoom != null) {
					currentDialog = new Dialog();
					currentRoom.responses.Add(currentDialog);
				}
			}
			if(currentBlock.blockType == BlockType.dt){
				if(currentRoom != null) {
					currentRoom.character = currentString.ToString();
				}
			}

			if(currentBlock.blockType == BlockType.dd){
				if(currentRoom != null) {
					currentRoom.text = currentString.ToString();
				}
			}













			if (currentBlock.blockType == BlockType.h2) {}

			if (currentBlock.blockType == BlockType.h3) {}

			if (currentBlock.blockType == BlockType.h4) {}

			if (currentBlock.blockType == BlockType.h5) {}

			if (currentBlock.blockType == BlockType.h6) {}

			if (currentBlock.blockType == BlockType.hr) {}

			if (currentBlock.blockType == BlockType.quote) {}

			if (currentBlock.blockType == BlockType.quote_end) {}

			if (currentBlock.blockType == BlockType.ul) {
				listStack.Push(currentBlock.blockType);
				//mdStyle.Begin_UnorderedList(container);
			}

			if (currentBlock.blockType == BlockType.ul_end) {
				listStack.Pop();
				//mdStyle.End_UnorderedList(container);
			}

			if (currentBlock.blockType == BlockType.ul_li) {
				//mdStyle.Create_UL_LI(container, currentString.ToString());
			}

			if (currentBlock.blockType == BlockType.codeblock) {}

			if(currentBlock.blockType == BlockType.table_spec){}



		};

		Dictionary<string, LinkDefinition> definitions;
			
		string htmlTranslation = md.Transform (markdownString, out definitions, (block, token, tokenString) => {

			if (block != null) {
				//Debug.Log ("block: " + block.blockType + " :: " + block.Content);

				CommitMarkdownBlock ();

				currentBlock = block;

				currentString.Length = 0;
			}

			if (token != null) {
				//Debug.Log ("token: " + token.type);

				if (token.type == TokenType.img) {
					//mdStyle.Create_IMG(container, link.def.url, link.link_text);
				}

				if (token.type == TokenType.Text) {
					currentString.Append (tokenString, token.startOffset, token.length);
				}


				if (token.type == TokenType.code_span) {
					//mdStyle.Tag_Code(container, currentString, true);
					//currentString.Append(tokenString, token.startOffset, token.length);
					//mdStyle.Tag_Code(container, currentString, false);
				}

				if (token.type == TokenType.open_strong) {
					//mdStyle.Tag_Strong(container, currentString, true);
				}

				if (token.type == TokenType.close_strong) {
					//mdStyle.Tag_Strong(container, currentString, false);
				}

				if (token.type == TokenType.br) {
					//mdStyle.Tag_BreakingReturn(container, currentString);
				}

				if (token.type == TokenType.link) {
					LinkInfo link = token.data as LinkInfo;
					if (currentDialog != null) {
						currentDialog.room = link.def.url;
						currentDialog.text = "\"" + link.link_text + "\"";
					}
				}

				if (token.type == TokenType.open_em) {
					//mdStyle.Tag_Emphasis(container, currentString, true);
				}

				if (token.type == TokenType.close_em) {
					//mdStyle.Tag_Emphasis(container, currentString, false);
				}


			}
		});

		if(currentRoom != null){
			AllRooms[currentRoom.hashtag] = currentRoom;
		}

		PrintAllRooms ();
	}

	// *********************************************************************************
}