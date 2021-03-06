﻿using System;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace STGCamera
{
    public class Mod : IUserMod
    {
        public string Name
        {
            get { return "Snap to Ground " + Version; }
        }

        public string Description
        {
            get { return "See your City from the Eyes of your Citizen"; }
        }

        public string Version
        {
            get { return "1.1"; }
        }
    }

    public class ModTerrainUtil : TerrainExtensionBase
    {

        private static ITerrain terrain = null;

        public static float GetHeight(float x, float z)
        {
            if (terrain == null)
            {
                return 0.0f;
            }

            return terrain.SampleTerrainHeight(x, z);
        }

        public override void OnCreated(ITerrain _terrain)
        {
            terrain = _terrain;
        }
    }

    public class ModLoad : LoadingExtensionBase
    {
        private UILabel cameraModeLabel;

        public override void OnLevelLoaded(LoadMode mode)
        {
            var uiView = GameObject.FindObjectOfType<UIView>();

            UIButton uibutton = uiView.AddUIComponent(typeof(UIButton)) as UIButton;

            uibutton.width = 36;
            uibutton.height = 36;

            uibutton.pressedBgSprite = "OptionBasePressed";
            uibutton.normalBgSprite = "OptionBase";
            uibutton.hoveredBgSprite = "OptionBaseHovered";
            uibutton.disabledBgSprite = "OptionBaseDisabled";

            uibutton.normalFgSprite = "InfoPanelIconFreecamera";
            uibutton.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            uibutton.scaleFactor = 1.0f;

            uibutton.tooltip = "STG Camera configuration";
            uibutton.tooltipBox = uiView.defaultTooltipBox;

            UIComponent escbutton = uiView.FindUIComponent("Esc");
            uibutton.relativePosition = new Vector2
            (
                escbutton.relativePosition.x + escbutton.width / 2.0f - uibutton.width / 2.0f - escbutton.width - 8.0f,
                escbutton.relativePosition.y + escbutton.height / 2.0f - uibutton.height / 2.0f
            );

            uibutton.eventClick += ButtonClick;

            var labelObject = new GameObject();
            labelObject.transform.parent = uiView.transform;

            cameraModeLabel = labelObject.AddComponent<UILabel>();
            cameraModeLabel.textColor = new Color32(255, 255, 255, 255);
            cameraModeLabel.transformPosition = new Vector3(1.15f, 0.90f);
            cameraModeLabel.Hide();

            STGCamera.Initialize();
            STGCamera.onCameraModeChanged = state =>
            {
                if (state)
                {
                    cameraModeLabel.text = String.Format("Press ({0}) to exit first-person mode", STGCamera.GetToggleUIKey());
                    cameraModeLabel.Show();
                }
                else
                {
                    cameraModeLabel.Hide();
                }
            };
        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            STGCamera.ToggleUI();
        }

    }

}
