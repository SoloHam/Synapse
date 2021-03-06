﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Synapse.Core.Templates;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using static Synapse.Core.Templates.Template;
using Syncfusion.Windows.Forms.Tools;

namespace Synapse.Controls
{
    public partial class AlignmentPipelineResultsControl : UserControl
    {
        #region Events
        public delegate void OnSelectedMethodResultChanged(AlignmentMethodResultControl alignmentMethodResultControl, Mat inputImage, Mat outputImg, Mat diffImg);
        public event OnSelectedMethodResultChanged OnSelectedMethodResultChangedEvent;
        #endregion

        #region Properties
        public AlignmentPipelineResults GetPipelineResults { get => pipelineResults; }
        private AlignmentPipelineResults pipelineResults; 
        #endregion

        #region Variables
        private SynchronizationContext synchronizationContext;
        private List<AlignmentMethodResultControl> alignmentMethodResultControls = new List<AlignmentMethodResultControl>();
        #endregion

        public AlignmentPipelineResultsControl()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
        }
        public AlignmentPipelineResultsControl(AlignmentPipelineResults pipelineResults)
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            this.pipelineResults = pipelineResults;

            Initialize(pipelineResults);
        }

        public void Initialize(AlignmentPipelineResults pipelineResults)
        {
            if (pipelineResults == null || pipelineResults.AlignmentMethodTestResultsList.Count == 0)
                return;

            pipelineResultsMainTabControl.TabPages.Clear();
            for (int i = 0; i < pipelineResults.AlignmentMethodTestResultsList.Count; i++)
            {
                AlignmentPipelineResults.AlignmentMethodResult alignmentMethodResult = pipelineResults.AlignmentMethodTestResultsList[i];
                AlignmentMethodResultControl alignmentMethodResultControl = new AlignmentMethodResultControl(alignmentMethodResult);

                TabPageAdv methodTabPageAdv = new TabPageAdv(alignmentMethodResult.AlignmentMethod.MethodName);
                methodTabPageAdv.Controls.Add(alignmentMethodResultControl);
                alignmentMethodResultControl.Dock = DockStyle.Fill;
                pipelineResultsMainTabControl.TabPages.Add(methodTabPageAdv);

                alignmentMethodResultControls.Add(alignmentMethodResultControl);
            }

            var selectedAlignmentMethodResultControl = alignmentMethodResultControls[pipelineResultsMainTabControl.SelectedIndex];
            selectedAlignmentMethodResultControl.GetResultImages(out Mat inputImg, out Mat outputImg, out Mat diffImg);

            OnSelectedMethodResultChangedEvent?.Invoke(selectedAlignmentMethodResultControl, inputImg, outputImg, diffImg);
        }

        private void PipelineResultsMainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedAlignmentMethodResultControl = alignmentMethodResultControls[pipelineResultsMainTabControl.SelectedIndex];
            selectedAlignmentMethodResultControl.GetResultImages(out Mat inputImg, out Mat outputImg, out Mat diffImg);

            OnSelectedMethodResultChangedEvent?.Invoke(selectedAlignmentMethodResultControl, inputImg, outputImg, diffImg);
        }
    }
}
