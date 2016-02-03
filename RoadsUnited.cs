﻿using ColossalFramework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




namespace RoadsUnited
{

    public class RoadsUnited : MonoBehaviour

    {


        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();

        public static Texture2D LoadTextureDDS(string fullPath)
        {
            // Testen ob Textur bereits geladen, in dem Fall geladene Textur zurückgeben
            Texture2D texture;
            if (textureCache.TryGetValue(fullPath, out texture)) return texture;

            // Nein? Textur laden
            var numArray = File.ReadAllBytes(fullPath);
            var width = BitConverter.ToInt32(numArray, 16);
            var height = BitConverter.ToInt32(numArray, 12);

            texture = new Texture2D(width, height, TextureFormat.DXT5, true);
            var list = new List<byte>();

            for (int index = 0; index < numArray.Length; ++index)
            {
                if (index > (int)sbyte.MaxValue)
                    list.Add(numArray[index]);
            }

            texture.LoadRawTextureData(list.ToArray());
            texture.name = Path.GetFileName(fullPath);
            texture.anisoLevel = 8;
            texture.Apply();

            textureCache.Add(fullPath, texture); // Neu geladene Textur in den Cache packen

            return texture;
        }

        public static void ReplaceNetTextures(string textureDir)
        {
            //Replace node textures
            NetCollection[] array = UnityEngine.Object.FindObjectsOfType<NetCollection>();
            NetCollection[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                NetCollection netCollection = array2[i];
                NetInfo[] prefabs = netCollection.m_prefabs;
                for (int j = 0; j < prefabs.Length; j++)
                {
                    NetInfo netInfo = prefabs[j];
                    string prefab_road_name = netInfo.name.Replace(" ", "_").ToLowerInvariant().Trim();

                    if (!(prefab_road_name.Contains("tl") || prefab_road_name.Contains("3l") || prefab_road_name.Contains("4l") || prefab_road_name.Contains("rural") || prefab_road_name.Contains("four-lane") || prefab_road_name.Contains("five-lane") || prefab_road_name.Contains("avenue") || prefab_road_name.Contains("large_highway")))
                    {


                        #region.Node Textures

                        NetInfo.Node[] nodes = netInfo.m_nodes;
                        for (int k = 0; k < nodes.Length; k++)
                        {
                            NetInfo.Node node = nodes[k];
                            string prefab_node_name_n = Path.Combine(textureDir, prefab_road_name + "_n.dds");
                            string prefab_node_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, prefab_road_name + "_n_map.dds");
                            string node_mesh_name_n = Path.Combine(textureDir, node.m_mesh.name.ToLowerInvariant() + "_n.dds");
                            string node_mesh_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, node.m_mesh.name.ToLowerInvariant() + "_n_map.dds");

                            // Begin replacing nodes

                            if (prefab_road_name.Contains("oneway"))
                            {
                                bool prefab_road_name_LARGE = prefab_road_name.Contains("large");
                                bool prefab_road_name_MEDIUM = prefab_road_name.Contains("medium");
                                bool prefab_road_name_SMALL = prefab_road_name.Contains("basic") || prefab_road_name.Contains("small");

                                if (prefab_road_name_LARGE)
                                {
                                    prefab_node_name_n = Path.Combine(textureDir, "large_road_n.dds");
                                    prefab_node_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, "large_road_n_map.dds");
                                }

                                if (prefab_road_name_MEDIUM)
                                {
                                    prefab_node_name_n = Path.Combine(textureDir, "medium_road_n.dds");
                                    prefab_node_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, "medium_road_n_map.dds");
                                }
                                if (prefab_road_name_SMALL)
                                {
                                    prefab_node_name_n = Path.Combine(textureDir, "basic_road_n.dds");
                                    prefab_node_name_n_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, "basic_road_n_map.dds");
                                }
                                goto set_nodes;

                            }


                            if (prefab_road_name.Contains("highway"))
                            {

                                if ((node.m_mesh.name.Equals("HighwayRampNode1") || node.m_mesh.name.Equals("HighwayRampNode2") || netInfo.name.Equals("HighwayRamp") || netInfo.name.Equals("HighwayRampElevated")))
                                {

                                    node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwayrampnode_n.dds")));
                                    node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwayrampnode_n.dds")));
                                    node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampnode_n_map.dds")));
                                    node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampnode_n_map.dds")));
                                }

                                else // ((node.m_mesh.name.Equals("HighwayBarrierNode") || node.m_mesh.name.Equals("HighwayBarrierPavement") || node.m_mesh.name.Equals("HighwayBasePavement") || node.m_mesh.name.Equals("HighwayBridgeNode1") || (node.m_mesh.name.Equals("HighwayBridgeNode2"))))
                                {
                                    node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwaybasenode_n.dds")));
                                    node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwaybasenode_n.dds")));
                                    node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasenode_n_map.dds")));
                                    node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasenode_n_map.dds")));
                                }
                                node.m_lodMesh = null;
                            }

                        set_nodes:

                            if (File.Exists(prefab_node_name_n))
                            {


                                node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(prefab_node_name_n));
                                node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(prefab_node_name_n));

                                if (File.Exists(prefab_node_name_n_map))
                                {

                                    node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(prefab_node_name_n_map));
                                    node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(prefab_node_name_n_map));
                                }
                                goto nodes_end;
                            }

                            else
                            if (File.Exists(node_mesh_name_n))
                            {
                                node.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(node_mesh_name_n));
                                node.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(node_mesh_name_n));
                                if (File.Exists(node_mesh_name_n_map))
                                {
                                    node.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(node_mesh_name_n_map));
                                    node.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(node_mesh_name_n_map));
                                }
                            }

                        nodes_end:


                            node.m_lodRenderDistance = 2500;


                        }


                        #endregion
                    }
                    #region.Segment Textures
                    // Look for segments


                    NetInfo.Segment[] segments = netInfo.m_segments;
                    for (int l = 0; l < segments.Length; l++)
                    {
                        NetInfo.Segment segment = segments[l];
                        string roadname_segment_default = Path.Combine(textureDir, prefab_road_name + "_s.dds");
                        string roadname_segment_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, prefab_road_name + "_s_map.dds");
                        string roadname_segment_lod = Path.Combine(ModLoader.currentTexturesPath_lod_rgb, prefab_road_name + "_s_lod.dds");
                        string meshname_segment_default = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + ".dds");
                        string meshname_segment_map = Path.Combine(ModLoader.currentTexturesPath_apr_maps, segment.m_mesh.name.ToLowerInvariant() + "_map.dds");
                        string meshname_segment_lod = Path.Combine(ModLoader.currentTexturesPath_lod_rgb, segment.m_mesh.name.ToLowerInvariant() + "_lod.dds");
                        string meshname_segment_deco1 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco1_grass.dds");
                        string meshname_segment_deco2 = Path.Combine(textureDir, segment.m_mesh.name.ToLowerInvariant() + "_deco2_trees.dds");


                        // Begin replacing segment textures
                        if (!(prefab_road_name.Contains("tl") || prefab_road_name.Contains("3l") || prefab_road_name.Contains("4l") || prefab_road_name.Contains("rural") || prefab_road_name.Contains("four-lane") || prefab_road_name.Contains("five-lane") || prefab_road_name.Contains("avenue") || prefab_road_name.Contains("large_highway")))
                        {
                            if ((prefab_road_name.Contains("highway") && !segment.m_material.name.ToLower().Contains("cable")))
                            {

                                if (prefab_road_name.Equals("highwayramp") || prefab_road_name.Equals("highwayrampelevated"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwayrampsegment.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwayrampsegment.dds")));
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampsegment_map.dds")));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayrampsegment_map.dds")));
                                }
                                else if (prefab_road_name.Equals("highwayramp_slope"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwayramp_tunnel.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwayramp_tunnel.dds")));
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayramp_tunnel_slope_map.dds")));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwayramp_tunnel_slope_map.dds")));
                                }

                                else if ((prefab_road_name.Equals("highway_barrier") || prefab_road_name.Equals("highway_bridge") || prefab_road_name.Equals("highway_elevated")))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwaybasesegment.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highwaybasesegment.dds")));
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasesegment_map.dds")));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highwaybasesegment_map.dds")));
                                }
                                else if (prefab_road_name.Equals("highway_slope"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highway_tunnel.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "highway_tunnel.dds")));
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highway_tunnel_slope_map.dds")));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(Path.Combine(ModLoader.currentTexturesPath_apr_maps, "highway_tunnel_slope_map.dds")));
                                }

                                else goto use_segment_name;

                                segment.m_lodMesh = null;
                                goto finished;

                            }

                            if (prefab_road_name.Contains("medium"))
                            {
                                if (prefab_road_name.Contains("decoration_grass") || prefab_road_name.Contains("decoration_trees"))
                                {

                                    if ((prefab_road_name.Contains("decoration_grass") && File.Exists(meshname_segment_deco1)))
                                    {
                                        segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_deco1));
                                        segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_deco1));
                                        goto finished;
                                    }

                                    if ((prefab_road_name.Contains("decoration_trees") && File.Exists(meshname_segment_deco2)))
                                    {
                                        segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_deco2));
                                        segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_deco2));
                                        goto finished;
                                    }
                                }

                                if (prefab_road_name.Equals("medium_road_bus"))
                                {
                                    if (segment.m_mesh.name.Equals("RoadMediumSegmentBusSide"))
                                    {
                                        segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "roadmediumsegmentbusside-buslane.dds")));
                                        segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "roadmediumsegmentbusside-buslane.dds")));
                                        goto finished;
                                    }

                                    else if (segment.m_mesh.name.Equals("RoadMediumSegmentBusBoth"))
                                    {
                                        segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "roadmediumsegmentbusboth-buslane.dds")));
                                        segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "roadmediumsegmentbusboth-buslane.dds")));
                                        goto finished;
                                    }
                                }
                            }

                            //Exception for Large Bus Roads - Bus Stops

                            if (prefab_road_name.Equals("large_road_bus") && meshname_segment_default.Contains("bus") && File.Exists(meshname_segment_default))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_default));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_default));
                                goto finished;
                            }


                            if (prefab_road_name.Contains("large") && prefab_road_name.Contains("oneway"))
                            {
                                if (segment.m_mesh.name.Equals("LargeRoadSegmentBusSide"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "large_oneway_s_busside.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "large_oneway_s_busside.dds")));
                                    goto finished;
                                }
                                else if (segment.m_mesh.name.Equals("LargeRoadSegment2BusSide"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "large_oneway_s2_busside.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "large_oneway_s2_busside.dds")));
                                    goto finished;
                                }

                                else if (segment.m_mesh.name.Equals("LargeRoadSegment2") && prefab_road_name.Contains("decoration"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "large_oneway_s2.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "large_oneway_s2.dds")));
                                    goto finished;
                                }


                                /*
                                if (segment.m_mesh.name.Equals("LargeRoadSegmentBusBothBuslane"))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "largeroadsegmentbusboth-bus.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "largeroadsegmentbusboth-bus.dds")));
                                }
                                */
                            }

                            if ((prefab_road_name.Contains("bus") || prefab_road_name.Contains("bike") || prefab_road_name.Contains("bicycle") || prefab_road_name.Contains("oneway")) && !prefab_road_name.Contains("tunnel") & File.Exists(roadname_segment_default))
                            {
                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(roadname_segment_default));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(roadname_segment_default));

                                if (File.Exists(roadname_segment_map))
                                {
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(roadname_segment_map));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(roadname_segment_map));
                                }

                                if (File.Exists(roadname_segment_lod))
                                {
                                    segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, roadname_segment_lod)));
                                }
                                goto finished;
                            }

                            if ((prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge")) && prefab_road_name.Contains("bus"))
                            {
                                if ((prefab_road_name.Contains("medium") && !meshname_segment_default.Contains("suspension")))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "mediumroadelevatedsegmentbus.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "mediumroadelevatedsegmentbus.dds")));
                                    goto finished;
                                }

                                else if ((prefab_road_name.Contains("large") && !meshname_segment_default.Contains("suspension")))
                                {
                                    segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "largeroadelevatedsegmentbus.dds")));
                                    segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "largeroadelevatedsegmentbus.dds")));
                                    goto finished;
                                }
                            }

                        use_segment_name:
                            if (File.Exists(roadname_segment_default))
                            {

                                segment.m_material.SetTexture("_Maintex", RoadsUnited.LoadTextureDDS(roadname_segment_default));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(roadname_segment_default));


                                if (File.Exists(roadname_segment_map))
                                {
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(roadname_segment_map));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(roadname_segment_map));
                                }

                                goto finished;
                            }

                            else if (File.Exists(meshname_segment_default))
                            {

                                segment.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_default));
                                segment.m_segmentMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_default));

                                if (File.Exists(meshname_segment_lod))
                                {
                                    segment.m_lodMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(meshname_segment_lod));
                                }

                                if (File.Exists(meshname_segment_map))
                                {
                                    segment.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(meshname_segment_map));
                                    segment.m_segmentMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(meshname_segment_map));
                                }
                            }






                        //     segment.m_combinedMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(Path.Combine(textureDir, "RoadLOD.dds")));


                        finished:

                            segment.m_lodRenderDistance = 2500;


                            #endregion

                            #region.Node Exceptions elevated


                            bool flag11 = (prefab_road_name.Contains("bus") || prefab_road_name.Contains("bike") || prefab_road_name.Contains("bicycle")) && !prefab_road_name.Contains("tunnel");
                            if (flag11)
                            {
                                NetInfo.Node[] nodes2 = netInfo.m_nodes;
                                for (int m = 0; m < nodes2.Length; m++)
                                {
                                    NetInfo.Node node2 = nodes2[m];
                                    string text9 = Path.Combine(textureDir, prefab_road_name.ToLowerInvariant() + ".dds");
                                    string text10 = Path.Combine(textureDir, prefab_road_name.ToLowerInvariant() + ".dds");

                                    bool flag12 = prefab_road_name.Contains("basic");
                                    if (flag12)
                                    {
                                        bool flag13 = prefab_road_name.Contains("slope");
                                        if (flag13)
                                        {
                                            text9 = Path.Combine(textureDir, "basic_road_decoration_n.dds");
                                            text10 = Path.Combine(textureDir, "basic_road_decoration_n_map.dds");
                                        }
                                        else
                                        {
                                            bool flag14 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                            if (flag14)

                                            {
                                                if (prefab_road_name.Contains("bicycle"))
                                                {
                                                    text9 = Path.Combine(textureDir, "smallroadelevatedbikenode.dds");
                                                }
                                                else
                                                {
                                                    text9 = Path.Combine(textureDir, "basic_road_bicycle_n.dds");
                                                }

                                            }
                                        }
                                        bool flag15 = prefab_road_name.Contains("medium");
                                        if (flag15)
                                        {
                                            bool flag16 = prefab_road_name.Contains("slope");
                                            if (flag16)
                                            {
                                                text9 = Path.Combine(textureDir, "medium_road_n.dds");
                                                text10 = Path.Combine(textureDir, "medium_road_n_map.dds");
                                            }
                                            else
                                            {
                                                bool flag17 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                                if (flag17)
                                                {
                                                    text9 = Path.Combine(textureDir, "roadmediumelevatednode.dds");
                                                    text10 = Path.Combine(textureDir, "nan_map.dds");
                                                }
                                                else
                                                {
                                                    text9 = Path.Combine(textureDir, "medium_road_n.dds");
                                                    text10 = Path.Combine(textureDir, "medium_road_n_map.dds");
                                                }
                                            }
                                        }
                                        bool flag18 = prefab_road_name.Contains("large");
                                        if (flag18)
                                        {
                                            bool flag19 = prefab_road_name.Contains("slope");
                                            if (flag19)
                                            {
                                                text9 = Path.Combine(textureDir, "large_road_decoration_n.dds");
                                                text10 = Path.Combine(textureDir, "large_road_decoration_n_map.dds");
                                            }
                                            else
                                            {
                                                bool flag20 = prefab_road_name.Contains("elevated") || prefab_road_name.Contains("bridge");
                                                if (flag20)
                                                {
                                                    text9 = Path.Combine(textureDir, "largeroadelevatednode.dds");
                                                    text10 = Path.Combine(textureDir, "large_road_elevated_n_map.dds");
                                                }
                                                else
                                                {
                                                    text9 = Path.Combine(textureDir, "large_road_n.dds");
                                                    text10 = Path.Combine(textureDir, "large_road_n_map.dds");
                                                }
                                            }
                                        }
                                        bool flag21 = File.Exists(text9);
                                        if (flag21)
                                        {
                                            node2.m_material.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(text9));
                                            node2.m_nodeMaterial.SetTexture("_MainTex", RoadsUnited.LoadTextureDDS(text9));
                                        }
                                        bool flag22 = File.Exists(text10);
                                        if (flag22)
                                        {
                                            node2.m_material.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(text10));
                                            node2.m_nodeMaterial.SetTexture("_APRMap", RoadsUnited.LoadTextureDDS(text10));
                                        }
                                    }
                                }
                            }

                            #endregion

                        }
                    }
                }
            }
        }
    }
}






