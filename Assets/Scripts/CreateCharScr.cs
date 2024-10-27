﻿using System;
using UnityEngine;

public class CreateCharScr : mScreen, IActionListener
{
	public static CreateCharScr instance;

	private PopUp p;

	public static bool isCreateChar = false;

	public static TField tAddName;

	public static int indexGender;

	public static int indexHair;

	public static int selected;

	public static int[][] hairID = new int[3][]
	{
		new int[3] { 64, 30, 31 },
		new int[3] { 9, 29, 32 },
		new int[3] { 6, 27, 28 }
	};

	public static int[] defaultLeg = new int[3] { 2, 13, 8 };

	public static int[] defaultBody = new int[3] { 1, 12, 7 };

	private int yButton;

	private int disY;

	private int[] bgID = new int[3] { 0, 4, 8 };

	public int yBegin;

	private int curIndex;

	private int cx = 168;

	private int cy = 350;

	private int dy = 45;

	private int cp1;

	private int cf;

	private int popW;
	private int popH;
	public CreateCharScr()
	{
		try
		{
			if (!GameCanvas.lowGraphic)
			{
				loadMapFromResource(new sbyte[3] { 39, 40, 41 });
			}
			loadMapTableFromResource(new sbyte[3] { 39, 40, 41 });
		}
		catch (Exception ex)
		{
			Cout.LogError("Tao char loi " + ex.ToString());
		}
		popW = GameCanvas.w > 200 ? 200 : GameCanvas.w;
		popH = 200;
		if (GameCanvas.w <= 200)
		{
			GameScr.setPopupSize(128, 100);
			GameScr.popupX = (GameCanvas.w - 128) / 2;
			GameScr.popupY = 10;
			cy += 15;
			dy -= 15;
		}
		indexGender = 1;
		tAddName = new TField();
		tAddName.width = GameCanvas.loginScr.tfUser.width;
		if (GameCanvas.w < 200)
		{
			tAddName.width = 60;
		}
		tAddName.height = mScreen.ITEM_HEIGHT + 2;
		tAddName.x = GameCanvas.w / 2 - tAddName.width / 2;
		tAddName.y = GameCanvas.hh - popW / 2 + tAddName.height;
		if (GameCanvas.w < 200)
		{
			tAddName.x = GameScr.popupX + 45;
			tAddName.y = GameScr.popupY + 12;
		}
		else
		{
			tAddName.x = GameCanvas.w / 2 - tAddName.width / 2;
			tAddName.y = 35;
		}
		if (!GameCanvas.isTouch)
		{
			tAddName.isFocus = true;
		}
		tAddName.setIputType(TField.INPUT_TYPE_ANY);
		tAddName.showSubTextField = false;
		tAddName.strInfo = mResources.char_name;
		if (tAddName.getText().Equals("@"))
		{
			tAddName.setText(GameCanvas.loginScr.tfUser.getText().Substring(0, GameCanvas.loginScr.tfUser.getText().IndexOf("@")));
		}
		tAddName.name = mResources.char_name;
		indexGender = 1;
		indexHair = 0;
		center = new Command(mResources.NEWCHAR, this, 8000, null);
		center.x = GameCanvas.w - center.w - 8;
		center.y = GameCanvas.h - center.h;//GameCanvas.hh + popH / 2 - center.h;
		left = new Command(mResources.BACK, this, 8001, null);
		left.x = 0;//GameCanvas.hw - popW / 2;
		left.y = GameCanvas.h - center.h;
		if (!GameCanvas.isTouch)
		{
			right = tAddName.cmdClear;
		}
		yBegin = tAddName.y;
	}

	public static CreateCharScr gI()
	{
		if (instance == null)
		{
			instance = new CreateCharScr();
		}
		return instance;
	}

	public static void init()
	{
	}

	public static void loadMapFromResource(sbyte[] mapID)
	{
		Res.outz("newwwwwwwwww =============");
		DataInputStream dataInputStream = null;
		for (int i = 0; i < mapID.Length; i++)
		{
			dataInputStream = MyStream.readFile("/mymap/" + mapID[i]);
			MapTemplate.tmw[i] = (ushort)dataInputStream.read();
			MapTemplate.tmh[i] = (ushort)dataInputStream.read();
			Cout.LogError("Thong TIn : " + MapTemplate.tmw[i] + "::" + MapTemplate.tmh[i]);
			MapTemplate.maps[i] = new int[dataInputStream.available()];
			Cout.LogError("lent= " + MapTemplate.maps[i].Length);
			for (int j = 0; j < MapTemplate.tmw[i] * MapTemplate.tmh[i]; j++)
			{
				MapTemplate.maps[i][j] = dataInputStream.read();
			}
			MapTemplate.types[i] = new int[MapTemplate.maps[i].Length];
		}
	}

	public void loadMapTableFromResource(sbyte[] mapID)
	{
		if (GameCanvas.lowGraphic)
		{
			return;
		}
		DataInputStream dataInputStream = null;
		try
		{
			for (int i = 0; i < mapID.Length; i++)
			{
				dataInputStream = MyStream.readFile("/mymap/mapTable" + mapID[i]);
				Cout.LogError("mapTable : " + mapID[i]);
				short num = dataInputStream.readShort();
				MapTemplate.vCurrItem[i] = new MyVector();
				Res.outz("nItem= " + num);
				for (int j = 0; j < num; j++)
				{
					short id = dataInputStream.readShort();
					short num2 = dataInputStream.readShort();
					short num3 = dataInputStream.readShort();
					if (TileMap.getBIById(id) != null)
					{
						BgItem bIById = TileMap.getBIById(id);
						BgItem bgItem = new BgItem();
						bgItem.id = id;
						bgItem.idImage = bIById.idImage;
						bgItem.dx = bIById.dx;
						bgItem.dy = bIById.dy;
						bgItem.x = num2 * TileMap.size;
						bgItem.y = num3 * TileMap.size;
						bgItem.layer = bIById.layer;
						MapTemplate.vCurrItem[i].addElement(bgItem);
						if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
						{
							try
							{
								Image image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
								if (image == null)
								{
									BgItem.imgNew.put(bgItem.idImage + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
									Service.gI().getBgTemplate(bgItem.idImage);
								}
								else
								{
									BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
								}
							}
							catch (Exception)
							{
								Image image2 = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
								if (image2 == null)
								{
									image2 = Image.createRGBImage(new int[1], 1, 1, bl: true);
									Service.gI().getBgTemplate(bgItem.idImage);
								}
								BgItem.imgNew.put(bgItem.idImage + string.Empty, image2);
							}
							BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
						}
						if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
						{
							BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
						}
						bgItem.changeColor();
					}
					else
					{
						Res.outz("item null");
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Cout.println("LOI TAI loadMapTableFromResource" + ex2.ToString());
		}
	}

	public override void switchToMe()
	{
		LoginScr.isContinueToLogin = false;
		GameCanvas.menu.showMenu = false;
		GameCanvas.endDlg();
		base.switchToMe();
		indexGender = Res.random(0, 3);
		indexHair = Res.random(0, 3);
		doChangeMap();
		Char.isLoadingMap = false;
		tAddName.setFocusWithKb(isFocus: true);
	}

	public void doChangeMap()
	{
		TileMap.maps = new int[MapTemplate.maps[indexGender].Length];
		for (int i = 0; i < MapTemplate.maps[indexGender].Length; i++)
		{
			TileMap.maps[i] = MapTemplate.maps[indexGender][i];
		}
		TileMap.types = MapTemplate.types[indexGender];
		TileMap.pxh = MapTemplate.pxh[indexGender];
		TileMap.pxw = MapTemplate.pxw[indexGender];
		TileMap.tileID = MapTemplate.pxw[indexGender];
		TileMap.tmw = MapTemplate.tmw[indexGender];
		TileMap.tmh = MapTemplate.tmh[indexGender];
		TileMap.tileID = bgID[indexGender] + 1;
		TileMap.loadMainTile();
		TileMap.loadTileCreatChar();
		GameCanvas.loadBG(bgID[indexGender]);
		GameScr.loadCamera(fullmScreen: false, cx, cy);
	}

	public override void keyPress(int keyCode)
	{
		tAddName.keyPressed(keyCode);
	}

	public override void update()
	{
		cp1++;
		if (cp1 > 30)
		{
			cp1 = 0;
		}
		if (cp1 % 15 < 5)
		{
			cf = 0;
		}
		else
		{
			cf = 1;
		}
		tAddName.update();
		if (selected != 0)
		{
			tAddName.isFocus = false;
		}
	}

	public override void updateKey()
	{
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
		{
			selected--;
			if (selected < 0)
			{
				selected = mResources.MENUNEWCHAR.Length - 1;
			}
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
		{
			selected++;
			if (selected >= mResources.MENUNEWCHAR.Length)
			{
				selected = 0;
			}
		}
		if (selected == 0)
		{
			tAddName.isFocus = true;
			if (!GameCanvas.isTouch)
			{
				right = tAddName.cmdClear;
			}
			tAddName.update();
		}
		if (selected == 1)
		{
			tAddName.isFocus = true;
			if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
			{
				indexGender--;
				if (indexGender < 0)
				{
					indexGender = mResources.MENUGENDER.Length - 1;
				}
				doChangeMap();
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
			{
				indexGender++;
				if (indexGender > mResources.MENUGENDER.Length - 1)
				{
					indexGender = 0;
				}
				doChangeMap();
			}
			right = null;
		}
		if (selected == 2)
		{
			tAddName.isFocus = true;
			if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
			{
				indexHair--;
				if (indexHair < 0)
				{
					indexHair = mResources.hairStyleName[0].Length - 1;
				}
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
			{
				indexHair++;
				if (indexHair > mResources.hairStyleName[0].Length - 1)
				{
					indexHair = 0;
				}
			}
			right = null;
		}
		if (GameCanvas.isPointerJustRelease)
		{

			if (GameCanvas.isPointerHoldIn(tAddName.x, tAddName.y, tAddName.width, tAddName.height))
			{
				selected = 0;
				tAddName.isFocus = true;
			}
			int yy = tAddName.y + tAddName.height + 10;
			int bf = popW / 2 - 30;

			if (GameCanvas.isPointerHoldIn(GameCanvas.hw - bf - 19, yy + 10, 13, 16))
			{
				selected = 1;
				int num4 = indexGender;
				indexGender--;
				if (indexGender < 0)
				{
					indexGender = mResources.MENUGENDER.Length - 1;
				}
				if (indexGender > mResources.MENUGENDER.Length - 1)
				{
					indexGender = 0;
				}
				if (num4 != indexGender)
				{
					doChangeMap();
				}
			}

			if (GameCanvas.isPointerHoldIn(GameCanvas.hw + bf - 6, yy + 10, 13, 16))
			{
				selected = 1;
				int num4 = indexGender;
				indexGender++;
				if (indexGender < 0)
				{
					indexGender = mResources.MENUGENDER.Length - 1;
				}
				if (indexGender > mResources.MENUGENDER.Length - 1)
				{
					indexGender = 0;
				}
				if (num4 != indexGender)
				{
					doChangeMap();
				}
			}

			if (GameCanvas.isPointerHoldIn(GameCanvas.hw - bf - 19, yy + 40, 13, 16))
			{
				selected = 2;
				int num5 = indexHair;
				indexHair--;
				if (indexHair < 0)
				{
					indexHair = mResources.hairStyleName[0].Length - 1;
				}
				if (indexHair > mResources.hairStyleName[0].Length - 1)
				{
					indexHair = 0;
				}
				if (num5 != selected)
				{
					doChangeMap();
				}
			}

			if (GameCanvas.isPointerHoldIn(GameCanvas.hw + bf - 6, yy + 40, 13, 16))
			{
				selected = 2;
				int num5 = indexHair;
				indexHair++;
				if (indexHair < 0)
				{
					indexHair = mResources.hairStyleName[0].Length - 1;
				}
				if (indexHair > mResources.hairStyleName[0].Length - 1)
				{
					indexHair = 0;
				}
				if (num5 != selected)
				{
					doChangeMap();
				}
			}

		}
		if (!TouchScreenKeyboard.visible)
		{
			base.updateKey();
		}
		GameCanvas.clearKeyHold();
		GameCanvas.clearKeyPressed();
	}

	public override void paint(mGraphics g)
	{
		if (Char.isLoadingMap)
		{
			return;
		}
		GameCanvas.paintBGGameScr(g);
		g.translate(-GameScr.cmx, -GameScr.cmy - 24);
		if (!GameCanvas.lowGraphic)
		{
			for (int i = 0; i < MapTemplate.vCurrItem[indexGender].size(); i++)
			{
				BgItem bgItem = (BgItem)MapTemplate.vCurrItem[indexGender].elementAt(i);
				if (bgItem.idImage != -1 && bgItem.layer == 1)
				{
					bgItem.paint(g);
				}
			}
		}
		TileMap.paintTilemap(g);




		int num2 = hairID[indexGender][indexHair];
		int num3 = defaultLeg[indexGender];
		int num4 = defaultBody[indexGender];

		g.translate(-g.getTranslateX(), -g.getTranslateY());

		cx = GameCanvas.hw;
		cy = GameCanvas.hh + popH / 2 - 20;
		dy = 0;
		PopUp.paintPopUp(g, GameCanvas.hw - popW / 2, GameCanvas.hh - popW / 2, popW, popH, -1, true);
		tAddName.paint(g);

		int bf = 0, bf2 = 0;
		if (GameCanvas.gameTick % 10 == 0)
		{
			if (selected == 1)
				bf = 3 >> 1;
			else if (selected == 2)
				bf2 = 3 >> 1;

		}
		//vẽ class
		int yy = tAddName.y + tAddName.height + 10;
		mFont.tahoma_7b_blue.drawString(g, "Hành Tinh", GameCanvas.hw, yy, mFont.CENTER);

		g.drawImage(GameScr.arrow, GameCanvas.hw + popW / 2 - 30 + bf, yy + 10);
		g.drawRegion(GameScr.arrow, 0, 0, 13, 16, 2, GameCanvas.hw - (popW / 2 - 30) - 13 - bf, yy + 10, 0);
		mFont.tahoma_7b_dark.drawString(g, mResources.MENUGENDER[indexGender], GameCanvas.hw, yy + 15, mFont.CENTER);

		//vẽ tóc
		mFont.tahoma_7b_blue.drawString(g, "Ngoại Hình", GameCanvas.hw, yy + 30, mFont.CENTER);
		g.drawImage(GameScr.arrow, GameCanvas.hw + (popW / 2 - 30) + bf2, yy + 40);
		g.drawRegion(GameScr.arrow, 0, 0, 13, 16, 2, GameCanvas.hw - (popW / 2 - 30) - 13 - bf2, yy + 40, 0);
		mFont.tahoma_7b_dark.drawString(g, mResources.hairStyleName[indexGender][indexHair], GameCanvas.hw, yy + 45, mFont.CENTER);


		g.drawImage(TileMap.bong, cx, cy + dy, 3);
		Part part = GameScr.parts[num2];
		Part part2 = GameScr.parts[num3];
		Part part3 = GameScr.parts[num4];
		SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[cf][0][0]].id, cx + Char.CharInfo[cf][0][1] + part.pi[Char.CharInfo[cf][0][0]].dx, cy - Char.CharInfo[cf][0][2] + part.pi[Char.CharInfo[cf][0][0]].dy + dy, 0, 0);
		SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[cf][1][0]].id, cx + Char.CharInfo[cf][1][1] + part2.pi[Char.CharInfo[cf][1][0]].dx, cy - Char.CharInfo[cf][1][2] + part2.pi[Char.CharInfo[cf][1][0]].dy + dy, 0, 0);
		SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[cf][2][0]].id, cx + Char.CharInfo[cf][2][1] + part3.pi[Char.CharInfo[cf][2][0]].dx, cy - Char.CharInfo[cf][2][2] + part3.pi[Char.CharInfo[cf][2][0]].dy + dy, 0, 0);
		if (!GameCanvas.lowGraphic)
		{
			for (int j = 0; j < MapTemplate.vCurrItem[indexGender].size(); j++)
			{
				BgItem bgItem2 = (BgItem)MapTemplate.vCurrItem[indexGender].elementAt(j);
				if (bgItem2.idImage != -1 && bgItem2.layer == 3)
				{
					bgItem2.paint(g);
				}
			}
		}
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		mFont.tahoma_7b_white.drawString(g, mResources.server + " " + LoginScr.serverName, 5, 5, 0, mFont.tahoma_7b_dark);
		if (!TouchScreenKeyboard.visible)
		{
			base.paint(g);
		}
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
			case 8000:
				if (tAddName.getText().Equals(string.Empty))
				{
					GameCanvas.startOKDlg(mResources.char_name_blank);
					break;
				}
				if (tAddName.getText().Length < 5)
				{
					GameCanvas.startOKDlg(mResources.char_name_short);
					break;
				}
				if (tAddName.getText().Length > 15)
				{
					GameCanvas.startOKDlg(mResources.char_name_long);
					break;
				}
				InfoDlg.showWait();
				Service.gI().createChar(tAddName.getText(), indexGender, hairID[indexGender][indexHair]);
				break;
			case 8001:
				if (GameCanvas.loginScr.isLogin2)
				{
					GameCanvas.startYesNoDlg(mResources.note, new Command(mResources.YES, this, 10019, null), new Command(mResources.NO, this, 10020, null));
					break;
				}
				if (Main.isWindowsPhone)
				{
					GameMidlet.isBackWindowsPhone = true;
				}
				Session_ME.gI().close();
				GameCanvas.serverScreen.switchToMe();
				break;
			case 10020:
				GameCanvas.endDlg();
				break;
			case 10019:
				Session_ME.gI().close();
				GameCanvas.endDlg();
				GameCanvas.serverScreen.switchToMe();
				break;
		}
	}
}
