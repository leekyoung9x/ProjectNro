﻿using Functions.HandlerFunctions;
using System;
using System.Collections.Generic;

namespace Functions.AutoFunctions
{
	public class AutoPickFunctions : IActionListener, IChatable
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x00008832 File Offset: 0x00006A32
		public static AutoPickFunctions getInstance()
		{
			if (AutoPickFunctions._Instance == null)
			{
				AutoPickFunctions._Instance = new AutoPickFunctions();
			}
			return AutoPickFunctions._Instance;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0008DCB0 File Offset: 0x0008BEB0
		public static void update()
		{
			if ((GameScr.isAutoPlay && (GameScr.canAutoPlay || AutoPickMobHandler.IsTanSat)) || !AutoPickFunctions.isAutoPick)
			{
				return;
			}
			if (AutoPickFunctions.isNRDMap(TileMap.mapID))
			{
				try
				{
					for (int i = 0; i < GameScr.vItemMap.size(); i++)
					{
						ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
						int num = global::Math.abs(global::Char.myCharz().cx - itemMap.x);
						if ((itemMap.playerId == global::Char.myCharz().charID || itemMap.playerId == -1) && num <= 60 && itemMap != null && mSystem.currentTimeMillis() - AutoPickFunctions.lastTimePickedItem > 550L && AutoPickFunctions.isNRD(itemMap))
						{
							Service.gI().pickItem(itemMap.itemMapID);
							AutoPickFunctions.lastTimePickedItem = mSystem.currentTimeMillis();
							break;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			if (!AutoPickFunctions.isNRDMap(TileMap.mapID))
			{
				AutoPickFunctions.FocusToNearestItem();
				if (global::Char.myCharz().itemFocus != null)
				{
					AutoPickFunctions.PickIt();
				}
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0008DDBC File Offset: 0x0008BFBC
		public void onChatFromMe(string text, string to)
		{
			if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
			{
				ChatTextField.gI().isShow = false;
				return;
			}
			if (ChatTextField.gI().strChat.Equals(AutoPickFunctions.inputMaximumPickDistance[0]))
			{
				try
				{
					int num = int.Parse(ChatTextField.gI().tfChat.getText());
					AutoPickFunctions.maximumPickDistance = num;
					GameScr.info1.addInfo("Khoảng Cách Nhặt: " + num, 0);
				}
				catch
				{
					GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
				}
				AutoPickFunctions.ResetTF();
				return;
			}
			if (ChatTextField.gI().strChat.Equals(AutoPickFunctions.inputItemID[0]))
			{
				try
				{
					int num2 = int.Parse(ChatTextField.gI().tfChat.getText());
					AutoPickFunctions.listItemAutoPick.Add(num2);
					GameScr.info1.addInfo("Đã Thêm Item " + num2, 0);
				}
				catch
				{
					GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
				}
				AutoPickFunctions.ResetTF();
				return;
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00006FE6 File Offset: 0x000051E6
		public void onCancelChat()
		{
			if (GameScr.isPaintMessage)
			{
				GameScr.isPaintMessage = false;
				ChatTextField.gI().center = null;
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0008DF14 File Offset: 0x0008C114
		public void perform(int idAction, object p)
		{
			switch (idAction)
			{
				case 1:
					AutoPickFunctions.isAutoPick = !AutoPickFunctions.isAutoPick;
					AutoPickFunctions.pickByList = 0;
					GameScr.info1.addInfo("Auto Nhặt\n" + (AutoPickFunctions.isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
					return;
				case 2:
					AutoPickFunctions.isPickAll = !AutoPickFunctions.isPickAll;
					GameScr.info1.addInfo("Nhặt Tất Cả\n" + (AutoPickFunctions.isPickAll ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
					return;
				case 3:
					AutoPickFunctions.isAutoPick = !AutoPickFunctions.isAutoPick;
					AutoPickFunctions.pickByList = 1;
					GameScr.info1.addInfo("Nhặt Theo Danh Sách\n" + (AutoPickFunctions.isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
					return;
				case 4:
					AutoPickFunctions.isTeleportToItem = !AutoPickFunctions.isTeleportToItem;
					GameScr.info1.addInfo("Dịch Đến Item\n" + (AutoPickFunctions.isTeleportToItem ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
					return;
				case 5:
					ChatTextField.gI().strChat = AutoPickFunctions.inputMaximumPickDistance[0];
					ChatTextField.gI().tfChat.name = AutoPickFunctions.inputMaximumPickDistance[1];
					ChatTextField.gI().startChat2(AutoPickFunctions.getInstance(), string.Empty);
					return;
				case 6:
					if (AutoPickFunctions.listItemAutoPick.Count == 0)
					{
						GameScr.info1.addInfo("Danh Sách Trống!", 0);
					}
					if (AutoPickFunctions.listItemAutoPick.Count > 0)
					{
						string text = "";
						for (int i = 0; i < AutoPickFunctions.listItemAutoPick.Count; i++)
						{
							text = text + AutoPickFunctions.listItemAutoPick[i].ToString() + " ";
						}
						GameScr.info1.addInfo(text, 0);
						return;
					}
					return;
				case 7:
					AutoPickFunctions.listItemAutoPick.Clear();
					GameScr.info1.addInfo("Đã Clear Danh Sách Nhặt!", 0);
					return;
				case 8:
					ChatTextField.gI().strChat = AutoPickFunctions.inputItemID[0];
					ChatTextField.gI().tfChat.name = AutoPickFunctions.inputItemID[1];
					ChatTextField.gI().startChat2(AutoPickFunctions.getInstance(), string.Empty);
					return;
				case 9:
					AutoPickFunctions.listItemAutoPick.Add((int)global::Char.myCharz().itemFocus.template.id);
					GameScr.info1.addInfo(string.Concat(new string[]
					{
				"Đã thêm ",
				global::Char.myCharz().itemFocus.template.name,
				" [",
				global::Char.myCharz().itemFocus.template.id.ToString(),
				"]"
					}), 0);
					return;
				default:
					return;
			}
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0008E1B8 File Offset: 0x0008C3B8
		public static void ShowMenu()
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Auto Nhặt\n" + ((!AutoPickFunctions.isAutoPick || AutoPickFunctions.pickByList != 0) ? "[STATUS: OFF]" : "[STATUS: ON]"), AutoPickFunctions.getInstance(), 1, null));
			myVector.addElement(new Command("Nhặt Tất Cả\n" + (AutoPickFunctions.isPickAll ? "[STATUS: ON]" : "[STATUS: OFF]"), AutoPickFunctions.getInstance(), 2, null));
			MyVector myVector2 = myVector;
			string str = "Nhặt Theo Danh Sách\n";
			string str2;
			if (AutoPickFunctions.isAutoPick)
			{
				if (AutoPickFunctions.pickByList == 1)
				{
					str2 = "[STATUS: ON]";
					goto IL_8E;
				}
			}
			str2 = "[STATUS: OFF]";
			IL_8E:
			myVector2.addElement(new Command(str + str2, AutoPickFunctions.getInstance(), 3, null));
			myVector.addElement(new Command("Dịch Đến Item\n" + (AutoPickFunctions.isTeleportToItem ? "[STATUS: ON]" : "[STATUS: OFF]"), AutoPickFunctions.getInstance(), 4, null));
			myVector.addElement(new Command("Khoảng Cách Nhặt\n[" + AutoPickFunctions.maximumPickDistance + "]", AutoPickFunctions.getInstance(), 5, null));
			myVector.addElement(new Command("Xem Danh Sách Nhặt", AutoPickFunctions.getInstance(), 6, null));
			myVector.addElement(new Command("Clear Danh Sách Nhặt", AutoPickFunctions.getInstance(), 7, null));
			myVector.addElement(new Command("Thêm ItemID", AutoPickFunctions.getInstance(), 8, null));
			if (global::Char.myCharz().itemFocus != null)
			{
				myVector.addElement(new Command(string.Concat(new string[]
				{
				"Thêm: ",
				global::Char.myCharz().itemFocus.template.name,
				" [",
				global::Char.myCharz().itemFocus.template.id.ToString(),
				"] "
				}), AutoPickFunctions.getInstance(), 9, null));
			}
			GameCanvas.menu.startAt(myVector, 3);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00008556 File Offset: 0x00006756
		private static void ResetTF()
		{
			ChatTextField.gI().strChat = "Chat";
			ChatTextField.gI().tfChat.name = "chat";
			ChatTextField.gI().isShow = false;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00003CCC File Offset: 0x00001ECC
		private static void smethod_0()
		{
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00003CCC File Offset: 0x00001ECC
		private static void smethod_1()
		{
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0008E38C File Offset: 0x0008C58C
		public static void FocusToNearestItem()
		{
			if (global::Char.myCharz().itemFocus != null)
			{
				return;
			}
			for (int i = 0; i < GameScr.vItemMap.size(); i++)
			{
				ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
				int num = global::Math.abs(global::Char.myCharz().cx - itemMap.x);
				int num2 = global::Math.abs(global::Char.myCharz().cy - itemMap.y);
				if (num <= AutoPickFunctions.maximumPickDistance && num2 <= AutoPickFunctions.maximumPickDistance && AutoPickFunctions.isPickIt(itemMap) && itemMap.template.id != 673)
				{
					global::Char.myCharz().itemFocus = itemMap;
					return;
				}
			}
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0008E430 File Offset: 0x0008C630
		public static void PickIt()
		{
			if (mSystem.currentTimeMillis() - AutoPickFunctions.lastTimePickedItem < 550L || global::Char.myCharz().itemFocus == null)
			{
				return;
			}
			if (AutoPickFunctions.isTeleportToItem && !global::Char.isLockKey)
			{
				AutoPickFunctions.TeleportTo(global::Char.myCharz().itemFocus.x, global::Char.myCharz().itemFocus.y);
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				if (global::Char.myCharz().itemFocus.template.id != 673)
				{
					Service.gI().pickItem(global::Char.myCharz().itemFocus.itemMapID);
					AutoPickFunctions.lastTimePickedItem = mSystem.currentTimeMillis();
				}
				return;
			}
			if (global::Char.myCharz().cx < global::Char.myCharz().itemFocus.x)
			{
				global::Char.myCharz().cdir = 1;
			}
			else
			{
				global::Char.myCharz().cdir = -1;
			}
			int num = global::Math.abs(global::Char.myCharz().cx - global::Char.myCharz().itemFocus.x);
			int num2 = global::Math.abs(global::Char.myCharz().cy - global::Char.myCharz().itemFocus.y);
			if (num <= 40 && num2 < 40)
			{
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				if (global::Char.myCharz().itemFocus.template.id != 673)
				{
					Service.gI().pickItem(global::Char.myCharz().itemFocus.itemMapID);
					AutoPickFunctions.lastTimePickedItem = mSystem.currentTimeMillis();
					return;
				}
			}
			else
			{
				global::Char.myCharz().currentMovePoint = new MovePoint(global::Char.myCharz().itemFocus.x, global::Char.myCharz().itemFocus.y);
				global::Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0008E5F4 File Offset: 0x0008C7F4
		private static void TeleportTo(int a, int b)
		{
			global::Char.myCharz().cx = a;
			global::Char.myCharz().cy = b;
			Service.gI().charMove();
			global::Char.myCharz().cx = a;
			global::Char.myCharz().cy = b + 1;
			Service.gI().charMove();
			global::Char.myCharz().cx = a;
			global::Char.myCharz().cy = b;
			Service.gI().charMove();
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0008E664 File Offset: 0x0008C864
		private static bool isPickIt(ItemMap a)
		{
			if (AutoPickFunctions.isPickAll)
			{
				return true;
			}
			if (AutoPickFunctions.pickByList == 0)
			{
				return a.playerId == global::Char.myCharz().charID || a.playerId == -1;
			}
			return AutoPickFunctions.pickByList == 1 && AutoPickFunctions.listItemAutoPick.Contains((int)a.template.id) && (a.playerId == global::Char.myCharz().charID || a.playerId == -1);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x000086C2 File Offset: 0x000068C2
		private static bool isNRDMap(int a)
		{
			return a >= 85 && a <= 91;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000086D3 File Offset: 0x000068D3
		private static bool isNRD(ItemMap a)
		{
			return a.template.id >= 372 && a.template.id <= 378;
		}

		// Token: 0x0400139D RID: 5021
		private static AutoPickFunctions _Instance;

		// Token: 0x0400139E RID: 5022
		public static bool isAutoPick;

		// Token: 0x0400139F RID: 5023
		public static long lastTimePickedItem;

		// Token: 0x040013A0 RID: 5024
		private static int maximumPickDistance = 50;

		// Token: 0x040013A1 RID: 5025
		private static bool isTeleportToItem;

		// Token: 0x040013A2 RID: 5026
		private static bool isPickAll;

		// Token: 0x040013A3 RID: 5027
		public static int pickByList;

		// Token: 0x040013A4 RID: 5028
		private static List<int> listItemAutoPick = new List<int>();

		// Token: 0x040013A5 RID: 5029
		private static string[] inputMaximumPickDistance = new string[]
		{
		"Nhập khoảng cách nhặt",
		"khoảng cách (>50)"
		};

		// Token: 0x040013A6 RID: 5030
		private static string[] inputItemID = new string[]
		{
		"Nhập ID của item",
		"ID"
		};
	}
}